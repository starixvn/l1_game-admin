using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        [Post("/forwardwithtoken")]
        Task<ChatBotResult> ForwardMessage(string token, string conversationId, string message);
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

        private const string MessageChangedSetting = "CHANGE {0}: {1} has new {2}!!!{3} Details: {4}";

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
                settingName.ToUpperInvariant(),
                gameName,
                settingName,
                additionalDetails,
                string.Join(";\r\n", changedSettings));

            _ = Task.Run(() => chatBotApi.ForwardMessage(
                appSettings.ChatBotApiSettings.Token,
                appSettings.ChatBotApiSettings.ConversationIds.AdminNotifications,
                message));

            return await Task.FromResult(new ChatBotResult());
        }
    }
}