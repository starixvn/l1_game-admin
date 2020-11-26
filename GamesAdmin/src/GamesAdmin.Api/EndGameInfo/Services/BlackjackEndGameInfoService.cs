using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamesAdmin.Api.EndGameInfo.Request;
using GamesAdmin.Core.Models.EndGameInfo;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.EndGameInfo
{
    public class BlackjackEndGameInfoService : BaseEndGameInfoService<BlackjackRoundEntity, BlackjackBetEntity>, IEndGameInfoService
    {
        public BlackjackEndGameInfoService(IGenericRepository<BlackjackRoundEntity> roundRepository, IGenericRepository<BlackjackBetEntity> betRepository)
            : base(roundRepository, betRepository)
        {
        }

        public async Task<EndGameInfoResult> GetAsync(RetrieveEndGameInfoRequest request)
        {
            var bets = await GetBetsAsync(request.GameRoundId, request.ObCustId, request.SiteId);

            if (!bets.Any())
            {
                return EndGameInfoError.SB1001;
            }

            if (bets.Any(x => !x.Paid))
            {
                return EndGameInfoError.SB1005;
            }

            var round = await GetRoundAsync(bets.FirstOrDefault().Round);

            var totalBetAmount = bets.Sum(x => x.Bet);
            var totalBetWin = bets.Sum(x => x.Betwin);

            return new EndGameInfoResult
            {
                EndTime = round.Time,
                ValidBetAmount = totalBetAmount,
                TotalAmount = totalBetAmount,
                TotalWin = totalBetWin
            };
        }

        protected override async Task<IEnumerable<BlackjackBetEntity>> GetBetsAsync(long gameRoundId, int memberId, string siteId, 
            Expression<Func<BlackjackBetEntity, bool>> filter = null)
        {
            return (await betRepository.FilterByAsync(x => x.HandId == gameRoundId && x.MemberId == memberId && x.SiteId == siteId)).ToList();
        }
    }
}