using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.KenoProMax;

namespace GamesAdmin.Api.EndGameInfo
{
    public class KenoProMaxEndGameInfoService : BaseEndGameInfoService<KenoProMaxRoundEntity, KenoProMaxBetEntity>, IEndGameInfoService
    {
        public KenoProMaxEndGameInfoService(IGenericRepository<KenoProMaxRoundEntity> roundRepository, IGenericRepository<KenoProMaxBetEntity> betRepository)
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

            var totalBetAmount = bets.Sum(x => x.Big + x.Small + x.Odd + x.Even + x.Gold + x.Wood + x.Water + x.Fire + x.Earth);
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