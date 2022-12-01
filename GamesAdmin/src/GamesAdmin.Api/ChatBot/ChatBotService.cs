using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Core;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Configurations;
using Refit;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using UAParser;
using Sentry;
using System.Linq;

namespace GamesAdmin.Api.ChatBot
{
    public interface IChatBotApi
    {
        [Post("/forwardwithtoken")]
        Task<ChatBotResult> ForwardMessage(string token, string conversationId, string message);
    }

    public interface IChatBotService
    {
        Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request);

        Task<ChatBotResult> NotifyCreatePromotion(PromotionModel promotion);

        Task<ChatBotResult> NotifyUpdatePromotion(PromotionModel promotion);
    }

    public class ChatBotService : IChatBotService
    {
        private const string HighStakeMessageTemplate = "**{0}** - {1} - {2} - {3} - {4}";
        private const int MinBetAmount = 2000;
        private const string MessageAddPromotion = "Added new promotion for game **{0}**:\r\n{1}";
        private const string MessageUpdatePromotion = "Edited promotion for game **{0}**:\r\n{1}";
        private const string MessagePromotion = "**Status: **{0}\r\n**Boosted Odds**: {1}\r\n**Type**: {2}\r\n**From**: {3}\r\n**To**: {4}";

        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;
        private readonly IGameUserService gameUserService;
        private readonly ISystemSettings systemSettings;
        private readonly IGenericRepository<UserSessionEntity> usersessionRepository;
        private readonly ISentryClient sentry;
        private readonly Parser uaParser;

        public ChatBotService(IAppSettings appSettings, IChatBotApi chatBotApi, IGameUserService gameUserService, ISystemSettings systemSettings, IGenericRepository<UserSessionEntity> usersessionRepository, ISentryClient sentry)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
            this.gameUserService = gameUserService;
            this.systemSettings = systemSettings;
            this.usersessionRepository = usersessionRepository;
            this.sentry = sentry;
            uaParser = Parser.GetDefault();
        }

        public async Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request)
        {
            var devicePlatform = await GetDevicePlatform(request.ObCustId);
            if (request.Currency == "UUS" || request.Amount < MinBetAmount)
            {
                return await Task.FromResult<ChatBotResult>(default);
            }

            var msgBuilder = new StringBuilder();
            var game = GetGameName(request.GameTypeId);
            var charName = await gameUserService.GetCharacterNameAsync(request.ObCustId);
            var version = GetVersionName(Enum.Parse<Versions>(request.System.ToString()), request.Metadata);

            msgBuilder.Append(string.Format(HighStakeMessageTemplate, version, charName, request.Amount, game, devicePlatform));

            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, appSettings.ChatBotApiSettings.ConversationIds.HighStake, msgBuilder.ToString()));

            return await Task.FromResult(new ChatBotResult());
        }

        private string GetGameName(byte gameId)
        {
            switch (gameId)
            {
                case (byte)GameId.KenoMatch1:
                    return "KenoSoccer";

                case (byte)GameId.KenoMatch2:
                    return "KenoSoccer2";

                case (byte)GameId.KenoPower1:
                    return "KenoPower";

                default:
                    return Enum.Parse<GameId>(gameId.ToString()).ToString();
            }
        }

        private string GetVersionName(Versions version, Dictionary<string, string> metadata)
        {
            var systemPrefix = GetSystemPrefixName();

            return version switch
            {
                Versions.Mobi => $"{systemPrefix}M",
                Versions.Instant => $"{systemPrefix}I{GetModeName(metadata)}",
                Versions.Cocos => $"{systemPrefix}{GetCocosVersionSuffix()}{GetModeName(metadata)}",
                Versions.MegaKeno => $"{GetMegaKenoVersionSuffix()}{GetModeName(metadata)}",
                _ => string.Empty,
            };
        }

        private string GetMegaKenoVersionSuffix()
        {
            var systemType = systemSettings.System;

            return systemType switch
            {
                SystemType.SC1 => "MK",
                SystemType.VG2 => "VGK",
                _ => string.Empty
            };
        }

        private string GetCocosVersionSuffix()
        {
            var systemType = systemSettings.System;

            return systemType switch
            {
                SystemType.SC1 => "C",
                _ => string.Empty
            };
        }

        private string GetSystemPrefixName()
        {
            var systemType = systemSettings.System;

            return systemType switch
            {
                SystemType.SC1 => "S",
                SystemType.VG2 => "VG",
                _ => string.Empty
            };
        }

        private static string GetModeName(Dictionary<string, string> metadata)
        {
            if (metadata != null && metadata.ContainsKey("mode") && metadata["mode"] == ClientDisplayMode.Portrait.Value)
            {
                return $" - {ClientDisplayMode.Portrait.DisplayName}";
            }

            return string.Empty;
        }

        public async Task<ChatBotResult> NotifyCreatePromotion(PromotionModel promotion)
        {
            var message = GetMessagePromotion(MessageAddPromotion, promotion);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, appSettings.ChatBotApiSettings.ConversationIds.Promotion, message));
            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyUpdatePromotion(PromotionModel promotion)
        {
            var message = GetMessagePromotion(MessageUpdatePromotion, promotion);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, appSettings.ChatBotApiSettings.ConversationIds.Promotion, message));
            return await Task.FromResult(new ChatBotResult());
        }

        private string GetMessagePromotion(string template, PromotionModel promotion)
        {
            var gameName = Game.FromGameId(promotion.GameId).DisplayName;
            var promotionMessage = string.Format(
                MessagePromotion,
                promotion.Status,
                JsonSerializer.Serialize(promotion.Odds),
                promotion.IsDaily ? PromotionType.Daily : PromotionType.Consecutive,
                promotion.StartDateTime.ToStringAtGMT7(),
                promotion.EndDateTime.ToStringAtGMT7());
            return string.Format(template, gameName, promotionMessage);
        }


        private async Task<string> GetDevicePlatform(int playerId)
        {
            var devicePlatform = "Unknown";
            try
            {
                var userSession = (await usersessionRepository.FilterByAsync(userSession => userSession.PlayerId == playerId)).OrderByDescending(userSession => userSession.RecheckInTime).FirstOrDefault();
                var browserInfo = uaParser.Parse(userSession?.BrowserUserAgent);
                devicePlatform = browserInfo.OS.ToString();

            }
            catch (Exception ex)
            {
                sentry.CaptureMessage($"{ex} - {playerId}" , Sentry.Protocol.SentryLevel.Warning);
            }

            return devicePlatform;
        }
    }
}