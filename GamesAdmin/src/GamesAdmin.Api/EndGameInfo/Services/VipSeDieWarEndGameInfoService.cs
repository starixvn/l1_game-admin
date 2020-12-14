using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class VipSeDieWarEndGameInfoService : BaseEndGameInfoService<VipSeDieWarRoundEntity, VipSeDieWarOnesEntity>, IEndGameInfoService
    {
        public VipSeDieWarEndGameInfoService(IGenericRepository<VipSeDieWarRoundEntity> roundRepository, IGenericRepository<VipSeDieWarOnesEntity> betRepository)
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

            if (!bet.Paid)
            {
                return EndGameInfoError.SB1005;
            }

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = bet.TotalAmount,
                TotalAmount = bet.TotalAmount,
                TotalWin = bet.Betwin
            };
        }
    }
}