using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class BitcoinMasterEndGameInfoService : BaseEndGameInfoService<BitcoinMasterRoundEntity, BitcoinMasterOnesEntity>, IEndGameInfoService
    {
        public BitcoinMasterEndGameInfoService(IGenericRepository<BitcoinMasterRoundEntity> roundRepository, IGenericRepository<BitcoinMasterOnesEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request.GameRoundId);
            var bets = await GetBetsAsync(request.GameRoundId, request.ObCustId, request.SiteId);

            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var bet = bets.FirstOrDefault();
            var amount = bet.Up + bet.Down + bet.Odd + bet.Even;

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = amount,
                TotalAmount = amount,
                TotalWin = bet.Betwin > 0 ? bet.Betwin + amount : bet.Refund
            };
        }
    }
}