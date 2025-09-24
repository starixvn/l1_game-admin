using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.ChatBot.Results;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.ExchangeRates;
using Refit;

namespace GamesAdmin.Api.ChatBot
{
    public interface IChatBotApi
    {
        [Post("/forwardwithtoken?token={token}")]
        Task<ChatBotResult> ForwardMessage(string token, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> formData);
    }

    public interface IChatBotService
    {
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


        Task<ChatBotResult> NotifyAdminUpdatedSettings(string gameName, string settingName, string additionalDetails, List<SettingChangedModel> settings);

        Task<ChatBotResult> NotifyAdminAddSettings(string action, string settingName, object setting);
    }

    public class ChatBotService : IChatBotService
    {
        private const string MessageAddPromotion = "Added new promotion for game **{0}**:\r\n{1}";
        private const string MessageUpdatePromotion = "Edited promotion for game **{0}**:\r\n{1}";
        private const string MessagePromotion = "**Status: **{0}\r\n**Boosted Odds**: {1}\r\n**Type**: {2}\r\n**From**: {3}\r\n**To**: {4}";
        private const string MessageAddPromotionTournament = "Added new Promotion: **{0}**:\r\n{1}";
        private const string MessageEdiPromotionTournament = "Edit Promotion: **{0}**:\r\n{1}";

        private const int CacheExpireInMinutes = 60 * 24;

        private readonly IAppSettings appSettings;
        private readonly IChatBotApi chatBotApi;
        private readonly IGenericRepository<ExchangeRateEntity> exchangeRateRepository;
        private readonly ICacheManager cache;

        private const string MessageChangedSetting = "[{0}] CHANGE {1} ({2}):\r\n{3} has new {4}!!!\r\n{5} \r\nDetails: {6}";
        private const string MessageAddedSetting = "[{0}] {1} {2} ({3})\r\nDetails: {4}";

        private const string FormDataMessageKey = "Message";
        private const string FormDataConversationIdKey = "ConversationId";

        private static Dictionary<string, string> BuildFormData(string message, string conversationId)
        {
            return new Dictionary<string, string>
            {
                { FormDataMessageKey, message },
                { FormDataConversationIdKey, conversationId }
            };
        }

        private static string GetCurrentTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
        }

        public ChatBotService(
            IAppSettings appSettings,
            IChatBotApi chatBotApi,
            IGenericRepository<ExchangeRateEntity> exchangeRateRepository,
            ICacheManager cache)
        {
            this.appSettings = appSettings;
            this.chatBotApi = chatBotApi;
            this.exchangeRateRepository = exchangeRateRepository;
            this.cache = cache;
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

        public async Task<ChatBotResult> NotifyCreatePromotion(PromotionModel promotion)
        {
            var message = GetMessagePromotion(MessageAddPromotion, promotion);
            var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.Promotion);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyUpdatePromotion(PromotionModel promotion)
        {
            var message = GetMessagePromotion(MessageUpdatePromotion, promotion);
            var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.Promotion);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
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

            var formData = BuildFormData(message + messageListFailers, appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
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
            var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyUpdatePromotionTournament(PromotionConfigModel promotion, string type)
        {
            var message = GetMessagePromotionTournament(MessageEdiPromotionTournament, promotion, type);
            var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.PromotionTournament);
            _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyAdminUpdatedSettings(
            string gameName,
            string settingName,
            string additionalDetails,
            List<SettingChangedModel> settings)
        {
            var changedSettings = settings
                .Where(setting => setting.OldValue != setting.NewValue)
                .Select(setting => $"{setting.Name}: {setting.OldValue} -> {setting.NewValue}");

            if (!changedSettings.Any())
            {
                return await Task.FromResult(new ChatBotResult());
            }

            var message = string.Format(
                MessageChangedSetting,
                appSettings.Env,
                settingName.ToUpperInvariant(),
                GetCurrentTimestamp(),
                gameName,
                settingName,
                additionalDetails,
                string.Join(";\r\n", changedSettings));

            if (!string.IsNullOrWhiteSpace(appSettings.ChatBotApiSettings?.ConversationIds?.AdminNotifications))
            {
                var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.AdminNotifications);
                _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
            }

            return await Task.FromResult(new ChatBotResult());
        }

        public async Task<ChatBotResult> NotifyAdminAddSettings(
            string action,
            string settingName,
            object setting)
        {
            if (setting == null)
            {
                return await Task.FromResult(new ChatBotResult());
            }

            var serializedSetting = Newtonsoft.Json.JsonConvert.SerializeObject(setting, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                MaxDepth = 1,
            });

            var message = string.Format(
                MessageAddedSetting,
                appSettings.Env,
                action.ToUpperInvariant(),
                settingName.ToUpperInvariant(),
                GetCurrentTimestamp(),
                serializedSetting);

            if (!string.IsNullOrWhiteSpace(appSettings.ChatBotApiSettings?.ConversationIds?.AdminNotifications))
            {
                var formData = BuildFormData(message, appSettings.ChatBotApiSettings.ConversationIds.AdminNotifications);
                _ = Task.Run(() => chatBotApi.ForwardMessage(appSettings.ChatBotApiSettings.Token, formData));
            }

            return await Task.FromResult(new ChatBotResult());
        }
    }
}