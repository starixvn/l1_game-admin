using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class FishPrawnCrabEndGameInfoService : BaseEndGameInfoService<FishPrawnCrabRoundEntity, FishPrawnCrabBetEntity>, IEndGameInfoService
    {
        public FishPrawnCrabEndGameInfoService(IGenericRepository<FishPrawnCrabRoundEntity> roundRepository, IGenericRepository<FishPrawnCrabBetEntity> betRepository)
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

            var totalBetAmount = bets.Sum(x => (x.OldStag ?? x.Stag) + (x.OldRooster ?? x.Rooster) + (x.OldPrawn ?? x.Prawn) + (x.OldGourd ?? x.Gourd) + (x.OldCrab ?? x.Crab) + (x.OldFish ?? x.Fish));

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