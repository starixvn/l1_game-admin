using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class BigSmallTurboEndGameInfoService : BaseEndGameInfoService<BigSmallTurboRoundEntity, BigSmallTurboOnesEntity>, IEndGameInfoService
    {
        public BigSmallTurboEndGameInfoService(IGenericRepository<BigSmallTurboRoundEntity> roundRepository, IGenericRepository<BigSmallTurboOnesEntity> betRepository)
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

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = bet.Amount - bet.Refund,
                TotalAmount = bet.Amount,
                TotalWin = bet.Win ? bet.Betwin + bet.Amount : bet.Refund
            };
        }
    }
}