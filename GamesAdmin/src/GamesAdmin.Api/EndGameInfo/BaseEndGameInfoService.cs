using System;
using System.Collections.Generic;
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

    public abstract class BaseEndGameInfoService<TRounEntity, TBetEntity> where TRounEntity : BaseRoundEntity where TBetEntity : BaseBetEntity
    {
        private readonly IGenericRepository<TRounEntity> roundRepository;
        private readonly IGenericRepository<TBetEntity> betRepository;

        public BaseEndGameInfoService(IGenericRepository<TRounEntity> roundRepository, IGenericRepository<TBetEntity> betRepository)
        {
            this.roundRepository = roundRepository;
            this.betRepository = betRepository;
        }

        protected async Task<TRounEntity> GetRoundAsync(long gameRoundId)
        {
            var roundEntity = await roundRepository.FindOneAsync(x => x.Number == gameRoundId);

            return roundEntity;
        }

        protected async Task<IEnumerable<TBetEntity>> GetBetsAsync(long gameRoundId, int memberId, string siteId, Expression<Func<TBetEntity, bool>> filter = null)
        {
            Expression<Func<TBetEntity, bool>> defaultExpression = x => x.Round == gameRoundId && x.MemberId == memberId && x.SiteId == siteId && x.Paid;

            var finalExpression = filter != null ? defaultExpression.And(filter) : defaultExpression;
            var betEntities = await betRepository.FilterByAsync(finalExpression);

            return betEntities;
        }
    }
}