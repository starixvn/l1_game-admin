using System;
using System.Threading.Tasks;
using GamesAdmin.Api.Promotions.Requests;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database;
using Refit;
using Sentry;
using Sentry.Protocol;
using MongoDB.Driver;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Database.Entities.Promotion;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.Promotions
{
    [Headers("Authorization: Bearer")]
    public interface IAwardGameApi
    {
        [Post("/Player/Award")]
        Task<AwardApiResult> Award([Body] AwardWinningPlayer request);
    }

    public interface IAwardService
    {
        Task<AwardWinningPlayer> AwardWinningPlayer(AwardWinningPlayer awardWinningPlayer);

        Task<bool> AwardTournament(AwardTournamentRequest request);
    }

    public class AwardService : IAwardService
    {
        private const int AwardFailedErrorCode = 1;
        private readonly IAwardGameApi awardGameApi;
        private readonly IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository;
        private readonly IGenericRepository<PromotionConfigEntity> promotionConfigRepository;
        private readonly IGenericRepository<NotificationEntity> notificationRepository;
        private readonly IMapper mapper;
        private readonly IChatBotService chatBotService;
        private readonly ISentryClient sentryClient;

        public AwardService(IAwardGameApi awardGameApi,
            IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository,
            IGenericRepository<PromotionConfigEntity> promotionConfigRepository,
            IGenericRepository<NotificationEntity> notificationRepository,
            IMapper mapper,
            IChatBotService chatBotService,
            ISentryClient sentryClient)
        {
            this.awardGameApi = awardGameApi;
            this.promotionLeaderboardRepository = promotionLeaderboardRepository;
            this.promotionConfigRepository = promotionConfigRepository;
            this.notificationRepository = notificationRepository;
            this.mapper = mapper;
            this.chatBotService = chatBotService;
            this.sentryClient = sentryClient;
        }

        public async Task<AwardWinningPlayer> AwardWinningPlayer(AwardWinningPlayer awardWinningPlayer)
        {
            try
            {
                var apiResult = await awardGameApi.Award(awardWinningPlayer);
                if (apiResult.Result.IsSuccessful())
                {
                    sentryClient.CaptureMessage($"Award Winning Player: {Newtonsoft.Json.JsonConvert.SerializeObject(awardWinningPlayer)}", SentryLevel.Fatal);
                    var updated = await UpdateAwardedPlayer(awardWinningPlayer);
                    awardWinningPlayer.IsAwarded = updated;
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return awardWinningPlayer;
        }

        public async Task<bool> AwardTournament(AwardTournamentRequest request)
        {
            try
            {
                var winningPlayers = await promotionLeaderboardRepository.Collection()
                    .Find(e => e.IsAwarded != true && e.PromotionId == request.PromotionId && e.AwardedAmount > 0)
                    .SortByDescending(e => e.AwardedAmount)
                    .ToListAsync();
                var tasks = new List<Task<AwardWinningPlayer>>();
                foreach (var winningPlayer in winningPlayers)
                {
                    var player = mapper.Map<AwardWinningPlayer>(winningPlayer);
                    player.GameTypeId = 9999;
                    player.Amount = (decimal)player.AwardedAmount;
                    tasks.Add(AwardWinningPlayer(player));
                }
                var t = Task.WhenAll(tasks);
                await t;
                await NotifyConfirmPayment(request.PromotionId, t.Result.ToList());
                return !t.Result.Any(e => !e.IsAwarded);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        private async Task<bool> UpdateAwardedPlayer(AwardWinningPlayer awardWinningPlayer)
        {
            try
            {
                var entity = await promotionLeaderboardRepository.FindOneAsync(e => e.PlayerId == awardWinningPlayer.PlayerId);
                if (entity != null)
                {
                    var update = Builders<PromotionLeaderboardEntity>.Update
                        .Set(field => field.IsAwarded, true)
                        .Set(field => field.AwardedTime, DateTime.UtcNow);
                    await promotionLeaderboardRepository.UpdateAsync(e => e.PlayerId == awardWinningPlayer.PlayerId, update);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        private async Task NotifyConfirmPayment(int promotionId, List<AwardWinningPlayer> awardedPlayers)
        {
            var promotion = await promotionConfigRepository.FindOneAsync(e => e.PromotionId == promotionId);
            var notification = await notificationRepository.FindOneAsync(e => e.PromotionId == promotion.Id.ToString());
            var winners = awardedPlayers.Where(e => e.IsAwarded);
            var failers = awardedPlayers.Where(e => !e.IsAwarded);
            _ = chatBotService.NotifyConfirmPaymentPromotionTournament(notification.Title[Language.EN], promotion.StartDateTime, promotion.EndDateTime, winners, failers);
        }
    }
}