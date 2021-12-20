using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using System.Linq;
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
            var round = await GetRoundAsync(request.GameRoundId);
            var bets = await GetBetsAsync(request);

            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var bet = bets.FirstOrDefault();
            var amount = bet.Ante + bet.Call + bet.Turn + bet.River;

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = amount,
                TotalAmount = amount,
                TotalWin = bet.Betwin + amount
            };
        }
    }
}
