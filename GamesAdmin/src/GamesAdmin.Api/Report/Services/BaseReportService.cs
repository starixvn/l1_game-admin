using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.UserInfo;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report.Services
{
    public abstract class BaseReportService
    {
        protected readonly IUserInfoService userInfoService;
        protected readonly ICurrencyGroupService currencyGroupService;
        protected readonly CloudSettings cloudSettings;
        protected readonly IReportApi reportApi;

        public BaseReportService(IUserInfoService userInfoService,
            ICurrencyGroupService currencyGroupService,
            IServiceProvider serviceProvider,
            IAppSettings appSettings)
        {
            this.userInfoService = userInfoService;
            this.currencyGroupService = currencyGroupService;
            cloudSettings = appSettings.CloudSettings;
            if (!appSettings.CloudSettings?.IsCloudService == true)
            {
                reportApi = serviceProvider.GetService<IReportApi>();
            }
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

        protected async Task<BetHistory> GetBetHistoryOnCloud(BetHistoryFilter filter)
        {
            var betHistory = await reportApi.GetBetHistory(filter);
            betHistory.FractionalOddsFormat = await currencyGroupService.IsFractionalOddsFormat(filter.PartnerId, betHistory.CurrencyName);
            return betHistory;
        }
    }
}