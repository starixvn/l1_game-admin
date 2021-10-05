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

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = bet.Bet ?? bet.Amount,
                TotalAmount = bet.Bet ?? bet.Amount,
                TotalWin = bet.Betwin > 0 ? bet.Betwin + (bet.Bet ?? bet.Amount) : (bet.Back ?? bet.Refund)
            };
        }
    }
}