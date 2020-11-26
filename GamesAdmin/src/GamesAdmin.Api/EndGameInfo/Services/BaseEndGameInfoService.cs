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

    public abstract class BaseEndGameInfoService<TRounEntity, TBetEntity> where TRounEntity : BaseRoundEntity where TBetEntity : BaseBetEntity
    {
        protected readonly IGenericRepository<TRounEntity> roundRepository;
        protected readonly IGenericRepository<TBetEntity> betRepository;

        protected BaseEndGameInfoService(IGenericRepository<TRounEntity> roundRepository, IGenericRepository<TBetEntity> betRepository)
        {
            this.roundRepository = roundRepository;
            this.betRepository = betRepository;
        }

        protected virtual async Task<TRounEntity> GetRoundAsync(long gameRoundId)
        {
            var roundEntity = await roundRepository.FindOneAsync(x => x.Number == gameRoundId);

            return roundEntity;
        }

        protected virtual async Task<IEnumerable<TBetEntity>> GetBetsAsync(long gameRoundId, int memberId, string siteId, Expression<Func<TBetEntity, bool>> filter = null)
        {
            Expression<Func<TBetEntity, bool>> defaultExpression = x => x.Round == gameRoundId && x.MemberId == memberId && x.SiteId == siteId && x.Paid;

            var finalExpression = filter != null ? defaultExpression.And(filter) : defaultExpression;

            return (await betRepository.FilterByAsync(finalExpression)).ToList();
        }
    }
}