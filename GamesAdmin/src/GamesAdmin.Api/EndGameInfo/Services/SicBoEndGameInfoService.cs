using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class SicBoEndGameInfoService : BaseEndGameInfoService<SicBoRoundEntity, SicBoOnesEntity>, IEndGameInfoService
    {
        public SicBoEndGameInfoService(IGenericRepository<SicBoRoundEntity> roundRepository, IGenericRepository<SicBoOnesEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request);
            var one = (await GetBetsAsync(request)).FirstOrDefault();

            if (round == null || one == null)
            {
                return EndGameInfoError.SB1001;
            }

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = one.TotalAmount,
                TotalAmount = one.TotalAmount,
                TotalWin = one.Betwin
            };
        }
    }
}