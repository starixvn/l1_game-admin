using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class HiLoEndGameInfoService : BaseEndGameInfoService<HiLoRoundEntity, HiLoBetLogEntity>, IEndGameInfoService
    {
        public HiLoEndGameInfoService(IGenericRepository<HiLoRoundEntity> roundRepository, IGenericRepository<HiLoBetLogEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var round = await GetRoundAsync(request);

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
                ValidBetAmount = round.Amount,
                TotalAmount = round.Amount,
                TotalWin = round.Betwin
            };
        }

        protected override Expression<Func<HiLoRoundEntity, bool>> GetRoundFilterExpression(RetrieveEndGameInfoRequest request)
            => (round) => round.Number == request.GameRoundId && round.PartnerId == request.PartnerId;
    }
}