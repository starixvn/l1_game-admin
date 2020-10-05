using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Keno;

namespace GamesAdmin.Api.EndGameInfo
{
    public class KenoProVietEndGameInfoService : BaseEndGameInfoService<KenoProVietRoundEntity, KenoProVietOnesEntity>, IEndGameInfoService
    {
        public KenoProVietEndGameInfoService(IGenericRepository<KenoProVietRoundEntity> roundRepository, IGenericRepository<KenoProVietOnesEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var bets = await GetBetsAsync(request.GameRoundId, request.ObCustId, request.SiteId, null, false);

            if (!bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            if (bets.Any(x => !x.Paid))
            {
                return EndGameInfoError.SB1005;
            }

            var round = await GetRoundAsync(request.GameRoundId);


            var totalBetAmount = bets.Sum(x => x.ConvertAmountToDictionary().Sum(c => c.Value));
            var totalBetWin = bets.Sum(x => x.Betwin);

            return new EndGameInfoResult
            {
                EndTime = round?.Time,
                ValidBetAmount = totalBetAmount,
                TotalAmount = totalBetAmount,
                TotalWin = totalBetWin
            };
        }

        protected override async Task<IEnumerable<KenoProVietOnesEntity>> GetBetsAsync(long gameRoundId, int memberId, string siteId,
            Expression<Func<KenoProVietOnesEntity, bool>> filter = null)
        {
            return (await betRepository.FilterByAsync(x => x.Round == gameRoundId && x.MemberId == memberId && x.SiteId == siteId)).ToList();
        }
    }
}