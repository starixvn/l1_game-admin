using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class OddEvenTurboEndGameInfoService : BaseEndGameInfoService<OddEvenTurboRoundEntity, OddEvenTurboOnesEntity>, IEndGameInfoService
    {
        public OddEvenTurboEndGameInfoService(IGenericRepository<OddEvenTurboRoundEntity> roundRepository, IGenericRepository<OddEvenTurboOnesEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request);
            var bets = await GetBetsAsync(request);

            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var bet = bets.FirstOrDefault();
            var betAmount = bet.Bet ?? bet.Amount;

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = betAmount,
                TotalAmount = betAmount,
                TotalWin = bet.Win ? bet.Betwin + betAmount : (bet.Back ?? bet.Refund)
            };
        }
    }
}