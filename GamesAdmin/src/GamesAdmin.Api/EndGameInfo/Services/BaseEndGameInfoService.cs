using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public interface IEndGameInfoService
    {
        Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request);
    }

    public abstract class RoundOnlyEndGameInfoService<TRounEntity> where TRounEntity : BaseRoundEntity
    {
        protected readonly IGenericRepository<TRounEntity> roundRepository;

        protected RoundOnlyEndGameInfoService(IGenericRepository<TRounEntity> roundRepository)
        {
            this.roundRepository = roundRepository;
        }

        protected virtual async Task<TRounEntity> GetRoundAsync(long gameRoundId)
        {
            var roundEntity = await roundRepository.FindOneAsync(x => x.Number == gameRoundId);

            return roundEntity;
        }

        protected virtual async Task<TRounEntity> GetRoundAsync(RetrieveEndGameInfoRequest request)
        {
            var expression = GetRoundFilterExpression(request);
            var roundEntity = await roundRepository.FindOneAsync(expression);

            return roundEntity;
        }

        protected virtual Expression<Func<TRounEntity, bool>> GetRoundFilterExpression(RetrieveEndGameInfoRequest request)
            => (round) => round.Number == request.GameRoundId;
    }

    public abstract class BaseEndGameInfoService<TRounEntity, TBetEntity> : RoundOnlyEndGameInfoService<TRounEntity> where TRounEntity : BaseRoundEntity where TBetEntity : BaseBetEntity
    {
        protected readonly IGenericRepository<TBetEntity> betRepository;

        protected BaseEndGameInfoService(IGenericRepository<TRounEntity> roundRepository, IGenericRepository<TBetEntity> betRepository) : base(roundRepository)
        {
            this.betRepository = betRepository;
        }

        protected virtual async Task<IEnumerable<TBetEntity>> GetBetsAsync(RetrieveEndGameInfoRequest request)
        {
            var expression = GetBetFilterExpression(request);
            var partnerExpression = PartnerFilterExpression(request);
            var finalExpression = expression.And(partnerExpression);

            return (await betRepository.FilterByAsync(finalExpression)).ToList();
        }

        protected virtual Expression<Func<TBetEntity, bool>> GetBetFilterExpression(RetrieveEndGameInfoRequest request)
            => (bet) => bet.Round == request.GameRoundId && bet.MemberId == request.ObCustId && bet.SiteId == request.SiteId && bet.Paid;

        protected virtual Expression<Func<TBetEntity, bool>> PartnerFilterExpression(RetrieveEndGameInfoRequest request)
            => (bet) => string.IsNullOrEmpty(bet.PartnerId) || string.Equals(request.PartnerId, bet.PartnerId);

        protected virtual Expression<Func<TBetEntity, bool>> FreeBetExpression(RetrieveEndGameInfoRequest request)
            => bet => !bet.FreeBet.HasValue || bet.FreeBet.Value == request.FreeBet;
    }
}