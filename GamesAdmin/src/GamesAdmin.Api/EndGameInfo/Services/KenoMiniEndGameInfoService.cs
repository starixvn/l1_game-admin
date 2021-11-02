using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Keno;

namespace GamesAdmin.Api.EndGameInfo
{
    public class KenoMiniEndGameInfoService : BaseEndGameInfoService<KenoMiniRoundEntity, KenoMiniOnesEntity>, IEndGameInfoService
    {
        public KenoMiniEndGameInfoService(IGenericRepository<KenoMiniRoundEntity> roundRepository, IGenericRepository<KenoMiniOnesEntity> betRepository)
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

            var totalBetAmount = bets.Sum(x => x.ConvertAmountToDictionary().Sum(c => c.Value));
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