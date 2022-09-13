using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Core;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Configurations;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class ChatBotService : IChatBotService
    {
        private const string Separator = " - ";
        private const string HighStakeMessageTemplate = "**{0}** - {1} - {2}";
        private const int MinBetAmount = 2000;

        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;
        private readonly IGameUserService gameUserService;
        private readonly ISystemSettings systemSettings;

        public ChatBotService(IAppSettings appSettings, IChatBotApi chatBotApi, IGameUserService gameUserService, ISystemSettings systemSettings)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
            this.gameUserService = gameUserService;
            this.systemSettings = systemSettings;
        }

        public async Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request)
        {
            if (request.Currency == "UUS" || request.Amount < MinBetAmount)
            {
                return await Task.FromResult<ChatBotResult>(default);
            }

            var msgBuilder = new StringBuilder();
            var game = Enum.Parse<GameId>(request.GameTypeId.ToString());
            var conversationSetting = appSettings.ChatBotApiSettings.HighStake;
            var charName = await gameUserService.GetCharacterNameAsync(request.ObCustId);
            var version = GetVersionName(Enum.Parse<Versions>(request.System.ToString()), request.Metadata);

            msgBuilder.Append(string.Format(HighStakeMessageTemplate, version, charName, request.Amount));
            msgBuilder.Append($"{Separator}{game}");

            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, conversationSetting.ConversationId, msgBuilder.ToString()));

            return await Task.FromResult(new ChatBotResult());
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
    }
}