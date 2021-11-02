using System;
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
    public class ThreeCardsEndGameInfoService : BaseEndGameInfoService<ThreeCardsRoundEntity, ThreeCardsBetEntity>, IEndGameInfoService
    {
        public ThreeCardsEndGameInfoService(IGenericRepository<ThreeCardsRoundEntity> roundRepository, IGenericRepository<ThreeCardsBetEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request.GameRoundId);
            var bets = await GetBetsAsync(request);

            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var totalBetAmount = bets.Sum(x => x.Red + x.Black + x.Diamonds + x.Hearts + x.Spades + x.Clubs);
            var totalBetWin = bets.Sum(x => x.Betwin);

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = totalBetAmount,
                TotalAmount = totalBetAmount,
                TotalWin = totalBetWin
            };
        }

        protected override Expression<Func<ThreeCardsBetEntity, bool>> GetBetFilterExpression(RetrieveEndGameInfoRequest request)
           => base.GetBetFilterExpression(request)
               .And(FreeBetExpression(request));

        private static Expression<Func<ThreeCardsBetEntity, bool>> FreeBetExpression(RetrieveEndGameInfoRequest request)
            => bet => !bet.FreeBet.HasValue || bet.FreeBet.Value == request.FreeBet;
    }
}