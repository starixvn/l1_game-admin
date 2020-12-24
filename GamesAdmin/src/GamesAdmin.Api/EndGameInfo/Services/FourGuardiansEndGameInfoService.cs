using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class FourGuardiansEndGameInfoService : IEndGameInfoService
    {
        private readonly IGenericRepository<FourGuardiansRoundEntity> roundRepository;

        public FourGuardiansEndGameInfoService(IGenericRepository<FourGuardiansRoundEntity> roundRepository)
        {
            this.roundRepository = roundRepository;
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = (await roundRepository
                .FilterByAsync(x => x.Number == request.GameRoundId
                    && x.MemberId == request.ObCustId
                    && x.SiteId == request.SiteId
                    && x.FreeSpin == request.FreeBet)).FirstOrDefault();

            if (round == null)
            {
                return EndGameInfoError.SB1001;
            }

            if(!round.Paid)
            {
                return EndGameInfoError.SB1005;
            }

            return new EndGameInfoResult
            {
                EndTime = round.EndTime,
                ValidBetAmount = round.TotalAmount,
                TotalAmount = request.FreeBet ? 0 : round.TotalAmount,
                TotalWin = round.BetWin
            };
        }
    }
}