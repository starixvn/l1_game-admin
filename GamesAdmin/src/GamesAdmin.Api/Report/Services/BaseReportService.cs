using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.UserInfo;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Api.Report.Services
{
    public abstract class BaseReportService
    {
        protected readonly IUserInfoService userInfoService;

        public BaseReportService(IUserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        protected async Task<IEnumerable<TBetRecord>> GetBetRecordsWithUserInfo<TBetRecord>(IEnumerable<TBetRecord> bets, string username, bool excludeBot)
            where TBetRecord : BetRecord
        {
            if (bets.Any())
            {
                var userInfos = await userInfoService.FilterByUidsAndUserName(bets.Select(x => x.UserId), username, excludeBot);

                return bets.Join(userInfos, bet => bet.UserId, userInfo => userInfo.Uid, (bet, userInfo) =>
                {
                    bet.Username = userInfo.Username;
                    bet.Nickname = userInfo.NickName;
                    return bet;
                });
            }
            return Enumerable.Empty<TBetRecord>();
        }
    }
}