using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class SeDieEndGameInfoService : BaseEndGameInfoService<SeDieRoundEntity, SeDieBetEntity>, IEndGameInfoService
    {
        public SeDieEndGameInfoService(IGenericRepository<SeDieRoundEntity> roundRepository, IGenericRepository<SeDieBetEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request.GameRoundId);
            var bets = await GetBetsAsync(request.GameRoundId, request.ObCustId, request.SiteId, x=> !x.FreeBet.HasValue || x.FreeBet.Value == request.FreeBet);
            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var totalBetAmount = bets.Sum(x => x.Odd + x.Even + x.ThreeRed + x.FourRed + x.ThreeWhite + x.FourWhite + x.OldOdd + x.OldEven + x.TwoRed);
            var totalBetWin = bets.Sum(x => x.Betwin);

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = totalBetAmount,
                TotalAmount = request.FreeBet ? 0 : totalBetAmount,
                TotalWin = totalBetWin
            };
        }
    }
}