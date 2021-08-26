using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class KenoPowerEndGameInfoService<TKenoPowerRoundEntity, TKenoPowerOneEntity> : BaseEndGameInfoService<TKenoPowerRoundEntity, TKenoPowerOneEntity>, IEndGameInfoService
        where TKenoPowerRoundEntity: KenoPowerRoundEntity
        where TKenoPowerOneEntity: KenoPowerOneEntity
    {
        public KenoPowerEndGameInfoService(IGenericRepository<TKenoPowerRoundEntity> roundRepository, IGenericRepository<TKenoPowerOneEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request.GameRoundId);
            var bets = await GetBetsAsync(request.GameRoundId, request.ObCustId, request.SiteId, x => !x.FreeBet.HasValue || x.FreeBet.Value == request.FreeBet);

            if (round == null || !bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            var totalBetAmount = bets.Sum(x => x.TotalAmount);
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