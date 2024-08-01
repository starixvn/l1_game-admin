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
using System.Linq.Expressions;

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
        Task<bool> AwardTournament(AwardTournamentRequest request);

        Task<bool> AwardTestCurrency(AwardTestCurrencyRequest request);
    }

    public class AwardService : IAwardService
    {
        private const int gameTypeIdTournament = 9999;
        private readonly IAwardGameApi awardGameApi;
        private readonly IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository;
        private readonly IGenericRepository<PromotionConfigEntity> promotionConfigRepository;
        private readonly IMapper mapper;
        private readonly IChatBotService chatBotService;
        private readonly ISentryClient sentryClient;

        public AwardService(IAwardGameApi awardGameApi,
            IGenericRepository<PromotionLeaderboardEntity> promotionLeaderboardRepository,
            IGenericRepository<PromotionConfigEntity> promotionConfigRepository,
            IMapper mapper,
            IChatBotService chatBotService,
            ISentryClient sentryClient)
        {
            this.awardGameApi = awardGameApi;
            this.promotionLeaderboardRepository = promotionLeaderboardRepository;
            this.promotionConfigRepository = promotionConfigRepository;
            this.mapper = mapper;
            this.chatBotService = chatBotService;
            this.sentryClient = sentryClient;
        }

        private async Task<AwardWinningPlayer> AwardWinningPlayer(AwardWinningPlayer awardWinningPlayer)
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

        public async Task<bool> AwardTestCurrency(AwardTestCurrencyRequest request)
        {
            try
            {
                var winningPlayer = await promotionLeaderboardRepository.Collection()
                    .Find(e => e.IsAwarded != true &&
                    e.PromotionId == request.PromotionId
                    && e.PlayerId == request.PlayerId
                    && e.OperatorId == request.OperatorId).SingleAsync();
                var player = mapper.Map<AwardWinningPlayer>(winningPlayer);
                player.GameTypeId = gameTypeIdTournament;
                player.Amount = (decimal)request.AwardAmount;
                var winner = await AwardWinningPlayer(player);
                _ = NotifyConfirmPayment(request.PromotionId, new List<AwardWinningPlayer> { winner });
                return winner.IsAwarded;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return false;
            }
        }

        public async Task<bool> AwardTournament(AwardTournamentRequest request)
        {
            try
            {
                var winningPlayers = await promotionLeaderboardRepository.Collection()
                    .Find(e => e.IsAwarded != true && e.PromotionId == request.PromotionId && e.AwardedAmount > 0)
                    .SortByDescending(p => p.PointInBaseCurrency)
                    .ThenBy(p => p.DataSourceTimeStamp)
                    .ToListAsync();

                var tasks = new List<Task<AwardWinningPlayer>>();
                foreach (var winningPlayer in winningPlayers)
                {
                    var player = mapper.Map<AwardWinningPlayer>(winningPlayer);
                    player.GameTypeId = gameTypeIdTournament;
                    player.Amount = (decimal)player.AwardedAmount;
                    tasks.Add(AwardWinningPlayer(player));
                }
                var t = Task.WhenAll(tasks);
                await t;
                _ = NotifyConfirmPayment(request.PromotionId, t.Result.ToList());
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
                Expression<Func<PromotionLeaderboardEntity, bool>> expression = e => e.PlayerId == awardWinningPlayer.PlayerId && e.PromotionId == awardWinningPlayer.PromotionId;
                var entity = await promotionLeaderboardRepository.FindOneAsync(expression);
                if (entity != null)
                {
                    var update = Builders<PromotionLeaderboardEntity>.Update
                        .Set(field => field.IsAwarded, true)
                        .Set(field => field.AwardedTime, DateTime.UtcNow);
                    await promotionLeaderboardRepository.UpdateAsync(expression, update);

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
            try
            {
                if (awardedPlayers?.Count > 0)
                {
                    var promotion = await promotionConfigRepository.FindOneAsync(e => e.PromotionId == promotionId);
                    var winners = awardedPlayers.Where(e => e.IsAwarded);
                    var failers = awardedPlayers.Where(e => !e.IsAwarded);
                    await chatBotService.NotifyConfirmPaymentPromotionTournament(promotion.Name, promotion.StartDateTime, promotion.EndDateTime, winners, failers);
                }
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }
        }
    }
}