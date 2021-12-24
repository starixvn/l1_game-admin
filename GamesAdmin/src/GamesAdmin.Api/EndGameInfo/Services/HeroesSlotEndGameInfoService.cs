using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class HeroesSlotEndGameInfoService : RoundOnlyEndGameInfoService<HeroesSlotRoundEntity>, IEndGameInfoService
    {
        public HeroesSlotEndGameInfoService(IGenericRepository<HeroesSlotRoundEntity> roundRepository) : base(roundRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request);

            if (round == null)
            {
                return EndGameInfoError.SB1001;
            }

            if (!round.Paid)
            {
                return EndGameInfoError.SB1005;
            }

            return new EndGameInfoResult
            {
                EndTime = round.EndTime,
                ValidBetAmount = round.FreeSpin ? 0 : round.TotalAmount,
                TotalAmount = round.FreeSpin ? 0 : round.TotalAmount,
                TotalWin = round.BetWin
            };
        }

        protected override Expression<Func<HeroesSlotRoundEntity, bool>> GetRoundFilterExpression(RetrieveEndGameInfoRequest request)
            => (round) => round.Number == request.GameRoundId && round.MemberId == request.ObCustId && round.SiteId == request.SiteId;
    }
}