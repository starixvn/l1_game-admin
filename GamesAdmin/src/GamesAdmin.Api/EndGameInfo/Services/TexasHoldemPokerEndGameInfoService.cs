using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamesAdmin.Api.EndGameInfo
{
    public class TexasHoldemPokerEndGameInfoService :
        BaseEndGameInfoService<TexasHoldemPokerRoundEntity, TexasHoldemPokerBetEntity>, IEndGameInfoService
    {
        public TexasHoldemPokerEndGameInfoService(
            IGenericRepository<TexasHoldemPokerRoundEntity> roundRepository,
            IGenericRepository<TexasHoldemPokerBetEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var bets = await GetBetsAsync(request);

            if (!bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var round = await GetRoundAsync(request.GameRoundId);
            if (!round.Ended)
            {
                return EndGameInfoError.SB1005;
            }

            var bet = bets.FirstOrDefault();
            var amount = bet.Ante + bet.Call + bet.Turn + bet.River;

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = amount,
                TotalAmount = amount,
                TotalWin = bet.Betwin
            };
        }

        protected override Expression<Func<TexasHoldemPokerBetEntity, bool>> GetBetFilterExpression(RetrieveEndGameInfoRequest request)
        {
            return (bet) => bet.Round == request.GameRoundId && bet.MemberId == request.ObCustId && bet.SiteId == request.SiteId;
        }
    }
}
