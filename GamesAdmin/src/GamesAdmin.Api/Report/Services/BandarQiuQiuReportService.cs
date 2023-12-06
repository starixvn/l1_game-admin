using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Report;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.Report.Services;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.UserInfo;

namespace GamesAdmin.Api.Report
{
    public class BandarQiuQiuReportService : BaseReportService, IGameReportService, IBetHistoryReportService
    {
        private readonly IBandarQiuQiuReportRepository repository;
        private readonly IMapper mapper;

        public BandarQiuQiuReportService(IBandarQiuQiuReportRepository repository,
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
            if (cloudSettings?.ShouldCallCloudService((int)filter.Game) == true)
            {
                var report = await reportApi.GetBetReport<BandarQiuQiuBetReport>(filter);
                report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
                return report;
            }
            else
            {
                var result = await repository.GetRecordAsync(filter);
                return new BandarQiuQiuBetReport
                {
                    RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                    Records = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetRecord>(x))
                };
            }
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            if (cloudSettings?.ShouldCallCloudService((int)filter.Game) == true)
            {
                return await GetBetHistoryOnCloud(filter);
            }
            else
            {
                var result = await repository.GetBetHistoryAsync(filter);

                return new BandarQiuQiuBetHistory
                {
                    RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                    Bets = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetHistoryRecord>(x)),
                    Odds = result.Item3,
                    CurrencyName = result.Item2.FirstOrDefault()?.Currency ?? string.Empty
                };
            }
        }
    }
}