using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.Report.Services;
using GamesAdmin.Api.UserInfo;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Report;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class BacBall2ReportService : BaseReportService, IGameReportService, IBetHistoryReportService
    {
        private readonly IBacBall2ReportRepository repository;
        private readonly IMapper mapper;

        public BacBall2ReportService(IBacBall2ReportRepository repository,
            IUserInfoService userInfoService,
            ICurrencyGroupService currencyGroupService,
            IServiceProvider serviceProvider,
            IAppSettings appSettings,
            IMapper mapper) : base(userInfoService, currencyGroupService, serviceProvider, appSettings)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            var result = await repository.GetRecordAsync(filter);
            var report = new BacBall2BetReport
            {
                RoundResult = mapper.Map<BacBall2Round>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<BacBall2BetRecord>(x))
            };

            report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
            return report;
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync(filter);

            var betHistory = new BacBall2BetHistory
            {
                RoundResult = mapper.Map<BacBall2Round>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<BacBall2BetHistoryRecord>(x)),
                Odds = result.Item3,
                CurrencyName = result.Item2.FirstOrDefault()?.Currency ?? string.Empty
            };
            if (!cloudSettings.IsCloudGame(filter.Game))
            {
                betHistory.FractionalOddsFormat = await currencyGroupService.IsFractionalOddsFormat(filter.PartnerId, betHistory.CurrencyName);
            }
            return betHistory;
        }
    }
}