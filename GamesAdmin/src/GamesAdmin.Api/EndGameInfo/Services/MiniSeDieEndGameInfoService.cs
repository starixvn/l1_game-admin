using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class MiniSeDieEndGameInfoService : BaseEndGameInfoService<MiniSeDieRoundEntity, MiniSeDieBetEntity>, IEndGameInfoService
    {
        public MiniSeDieEndGameInfoService(IGenericRepository<MiniSeDieRoundEntity> roundRepository, IGenericRepository<MiniSeDieBetEntity> betRepository)
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

            var totalBetAmount = bets.Sum(x => x.Odd + x.Even + x.ThreeRed + x.FourRed + x.ThreeWhite + x.FourWhite);
            var totalBetWin = bets.Sum(x => x.Betwin);

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = totalBetAmount,
                TotalAmount = totalBetAmount,
                TotalWin = totalBetWin
            };
        }
    }
}