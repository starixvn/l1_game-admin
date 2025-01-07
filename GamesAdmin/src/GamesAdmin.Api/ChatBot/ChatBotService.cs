using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.ChatBot.Requests;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.Public;
using GamesAdmin.Core;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using GamesAdmin.Database.Entities.ExchangeRates;
using GamesAdmin.Site._Shared.Configurations;
using Refit;
using Sentry;
using UAParser;

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

        Task<ChatBotResult> NotifyConfirmPaymentPromotionTournament(string promotionTitle,
            DateTime startDate,
            DateTime endDate,
            IEnumerable<AwardWinningPlayer> winners,
            IEnumerable<AwardWinningPlayer> failers);

        Task<ChatBotResult> NotifyCreatePromotionTournament(PromotionConfigModel promotion, string type);

        Task<ChatBotResult> NotifyUpdatePromotionTournament(PromotionConfigModel promotion, string type);

        ExchangeRateEntity GetExchangeRate();
    }

    public class ChatBotService : IChatBotService
    {
        private const string HighStakeMessageTemplate = "**{0}** - {1} - {2} - {3} - {4} {5}";
        private const string MessageAddPromotion = "Added new promotion for game **{0}**:\r\n{1}";
        private const string MessageUpdatePromotion = "Edited promotion for game **{0}**:\r\n{1}";
        private const string MessagePromotion = "**Status: **{0}\r\n**Boosted Odds**: {1}\r\n**Type**: {2}\r\n**From**: {3}\r\n**To**: {4}";
        private const string MessageAddPromotionTournament = "Added new Promotion: **{0}**:\r\n{1}";
        private const string MessageEdiPromotionTournament = "Edit Promotion: **{0}**:\r\n{1}";
        
        private const int CacheExpireInMinutes = 60 * 24;

        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;
        private readonly IGameUserService gameUserService;
        private readonly ISystemSettings systemSettings;
        private readonly IGenericRepository<UserSessionEntity> usersessionRepository;
        private readonly ISentryClient sentry;
        private readonly Parser uaParser;
        private readonly IGenericRepository<ExchangeRateEntity> exchangeRateRepository;
        private readonly IGenericRepository<CurrencyEntity> currencyRepository;
        private readonly IGenericRepository<AccountEntity> userRepository;
        private readonly ICacheManager cache;
        private readonly IGeneralGameSettingService generalGameSettingService;

        public ChatBotService(
            IAppSettings appSettings,
            IChatBotApi chatBotApi,
            IGameUserService gameUserService,
            ISystemSettings systemSettings,
            IGenericRepository<UserSessionEntity> usersessionRepository,
            ISentryClient sentry,
            IGenericRepository<ExchangeRateEntity> exchangeRateRepository,
            IGenericRepository<CurrencyEntity> currencyRepository,
            IGenericRepository<AccountEntity> userRepository,
            ICacheManager cache,
            IGeneralGameSettingService generalGameSettingService)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
            this.gameUserService = gameUserService;
            this.systemSettings = systemSettings;
            this.usersessionRepository = usersessionRepository;
            this.sentry = sentry;
            uaParser = Parser.GetDefault();
            this.exchangeRateRepository = exchangeRateRepository;
            this.currencyRepository = currencyRepository;
            this.userRepository = userRepository;
            this.cache = cache;
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<ChatBotResult> NotifyHighStake(NotifyHighStakeRequest request)
        {
            try
            {
                var highStakeAmount = GetHighStakeAmount();
                var amount = await GetAmount(request);

                if (request.Currency == "UUS" || amount < highStakeAmount)
                {
                    return await Task.FromResult<ChatBotResult>(default);
                }

                var msgBuilder = new StringBuilder();
                var game = await GetGameName(request.GameTypeId);
                var charName = await GetCharName(request);
                var version = GetVersionName(Enum.Parse<Versions>(request.System.ToString()), request.Metadata);

                var playerDeviceExtraInformation = await GetDeviceInformation(request.ObCustId);
                msgBuilder.Append(string.Format(HighStakeMessageTemplate, version, charName, request.Amount, request.Currency, game, playerDeviceExtraInformation));

                _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, appSettings.ChatBotApiSettings.ConversationIds.HighStake, msgBuilder.ToString()));

                return await Task.FromResult(new ChatBotResult());
            }
            catch (Exception e)
            {
                sentry.CaptureException(e);
                return await Task.FromResult(new ChatBotResult());
            }
        }

        private async Task<string> GetCharName(NotifyHighStakeRequest request)
        {
            return appSettings.CloudSettings.IsCloudService ?
               $"playerId: {request.ObCustId}" : await gameUserService.GetCharacterNameAsync(request.ObCustId);
        }

        private double GetHighStakeAmount()
        {
            var defaultMinBetAmount = 2000;
            return appSettings.CloudSettings.IsCloudService ? defaultMinBetAmount : appSettings.ChatBotApiSettings.HighStakeAmount;
        }

        private async Task<double> GetAmount(NotifyHighStakeRequest request)
        {
            return appSettings.CloudSettings.IsCloudService ? request.Amount : await CalAmountOnBaseCurrency(request);
        }

        private async Task<double> CalAmountOnBaseCurrency(NotifyHighStakeRequest request)
        {
            try
            {
                var exchangeRate = GetExchangeRate();
                var currencies = GetCurrencies();
                var user = await GetUser(request.ObCustId, request.OperatorId);
                var currency = currencies.Where(c => c.Enabled == true && c.Name == request.Currency && c.PartnerId == user.OperatorId).FirstOrDefault();

                if (currency == null)
                {
                    return request.Amount;
                }

                var rate = (double)exchangeRate.Code[currency.Code] * currency.Multiplier;
                return request.Amount * rate;
            }
            catch (Exception e)
            {
                sentry.CaptureException(e);
                return request.Amount;
            }
        }

        public ExchangeRateEntity GetExchangeRate()
        {
            var exchangeRate = cache.GetCache<ExchangeRateEntity>(CachedKeys.ExchangeRate.ToString());
            if (exchangeRate == default)
            {
                exchangeRate = this.exchangeRateRepository.AsQueryable().OrderByDescending(ex => ex.ExId).First();
                cache.SetCache(CachedKeys.ExchangeRate.ToString(), exchangeRate, CacheExpireInMinutes);
            }

            return exchangeRate;
        }

        private IEnumerable<CurrencyEntity> GetCurrencies()
        {
            var currencies = cache.GetCache<IEnumerable<CurrencyEntity>>(CachedKeys.Currencies.ToString());
            if (currencies == default)
            {
                currencies = currencyRepository.AsQueryable();
                cache.SetCache(CachedKeys.Currencies.ToString(), currencies, CacheExpireInMinutes);
            }

            return currencies;
        }

        private async Task<AccountEntity> GetUser(string ObCustId, string operatorId)
        {
            var userKey = string.Format("{0}{1}", CachedKeys.User, ObCustId);
            var user = cache.GetCache<AccountEntity>(userKey);
            if (user == default)
            {
                user = await userRepository.FindOneAsync(u => u.PlayerId == ObCustId && u.OperatorId == operatorId);
                cache.SetCache(userKey, user, CacheExpireInMinutes);
            }

            return user;
        }

        private async Task<string> GetGameName(int gameId)
        {
            return gameId switch
            {
                (int)GameId.KenoMatch1 => "KenoSoccer",
                (int)GameId.KenoMatch2 => "KenoSoccer2",
                (int)GameId.KenoPower1 => "KenoPower",
                _ => await generalGameSettingService.GetDisplayName(gameId),
            };
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
                Versions.App => $"{systemPrefix}A",
                Versions.Livepage => $"{systemPrefix}L",
                Versions.MobileDirect => $"{systemPrefix}D",
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
                SystemType.GN => "GN",
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

        private async Task<string> GetDeviceInformation(string playerId)
        {
            try
            {
                if (appSettings.CloudSettings.IsCloudService)
                {
                    return string.Empty;
                }

                var userSession = (await usersessionRepository
                    .FilterByAsync(userSession => userSession.PlayerId == playerId))
                    .OrderByDescending(userSession => userSession.RecheckInTime)
                    .FirstOrDefault();

                return $"- {uaParser.Parse(userSession?.BrowserUserAgent).OS} - {userSession?.Ip}";
            }
            catch (Exception ex)
            {
                sentry.CaptureMessage($"{ex} - {playerId}", Sentry.Protocol.SentryLevel.Warning);
                return "- Unknown";
            }
        }

        public async Task<ChatBotResult> NotifyConfirmPaymentPromotionTournament(string promotionTitle,
            DateTime startDate,
            DateTime endDate,
            IEnumerable<AwardWinningPlayer> winners,
            IEnumerable<AwardWinningPlayer> failers)
        {
            var template = "The administrator has confirmed the payment for the **{0}**:\r\n**From**: {1}\r\n**To**: {2}\r\n**List of winners**:\r\n\t\t{3}";
            var messageListWinners = string.Join("\r\n\t\t", winners.Select(e => $"- {e.PlayerName}/{e.PlayerId}, {e.Currency}, TotalPoints: {e.Point}, WinAmount: {e.Amount}"));
            var message = string.Format(template, promotionTitle, startDate.ToStringAtGMT7(), endDate.ToStringAtGMT7(), messageListWinners);

            var messageListFailers = failers.Any() ? "\r\n**The request fails**: " + string.Join(", ", failers.Select(e => $"{e.PlayerName}/{e.PlayerId}")) : string.Empty;

            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token,
                appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament,
                message + messageListFailers));
            return await Task.FromResult(new ChatBotResult());
        }

        private string GetMessagePromotionTournament(string template, PromotionConfigModel promotion, string type)
        {
            string messageListOperators = SettingHelper.FormatOperators(promotion.Operators);
            string messageListWinningPrizes = SettingHelper.FormatWinningPrizes(promotion.WinningPrizes);

            string promotionTournamentMessage;

            if (type.Equals(PromotionTournamentTypeUpdate.CreateTournament))
            {
                promotionTournamentMessage = string.Format(
                    "**List of operators**:\r\n {0}\r\n**Winning Prizes**:\r\n {1}\r\nStart Time: {2}\r\nEnd Time: {3}",
                    messageListOperators,
                    messageListWinningPrizes,
                    promotion.StartDateTime.ToStringAtGMT7(),
                    promotion.EndDateTime.ToStringAtGMT7());
            }
            else if (type.Equals(PromotionTournamentTypeUpdate.OperatorSetting))
            {
                promotionTournamentMessage = string.Format(
                    "**List of operators**:\r\n {0}",
                    messageListOperators);
            }
            else
            {
                promotionTournamentMessage = string.Format(
                    "**Winning Prizes**:\r\n {0}\r\n**Start Time**: {1}\r\n**End Time**: {2}",
                    messageListWinningPrizes,
                    promotion.StartDateTime.ToStringAtGMT7(),
                    promotion.EndDateTime.ToStringAtGMT7());
            }

            return string.Format(template, promotion.Name, promotionTournamentMessage);
        }
        public async Task<ChatBotResult> NotifyCreatePromotionTournament(PromotionConfigModel promotion, string type)
        {
            var message = GetMessagePromotionTournament(MessageAddPromotionTournament, promotion, type);         
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token,
               appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament,
               message));
            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyUpdatePromotionTournament(PromotionConfigModel promotion, string type)
        {
            var message = GetMessagePromotionTournament(MessageEdiPromotionTournament, promotion, type);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token,
               appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament,
               message));
            return await Task.FromResult(new ChatBotResult());
        }
    }
}