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
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public class BandarQiuQiuReportService : BaseReportService, IGameReportService, IBetHistoryReportService
    {
        private readonly IBandarQiuQiuReportRepository repository;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly CloudSettings cloudSettings;
        private readonly IReportApi reportApi;
        private readonly IMapper mapper;

        public BandarQiuQiuReportService(IBandarQiuQiuReportRepository repository,
            IUserInfoService userInfoService,
            ICurrencyGroupService currencyGroupService,
            IServiceProvider serviceProvider,
            IAppSettings appSettings,
            IMapper mapper) : base(userInfoService)
        {
            this.repository = repository;
            this.currencyGroupService = currencyGroupService;
            this.mapper = mapper;
            cloudSettings = appSettings.CloudSettings;
            if (!appSettings.CloudSettings?.IsCloudService == true)
            {
                reportApi = serviceProvider.GetService<IReportApi>();
            }
        }

        public async Task<BetReportBase> Get(BetReportFilter filter)
        {
            BetReport<BandarQiuQiuRoundResult, BandarQiuQiuBetRecord> report;
            if (cloudSettings?.ShouldCallCloudService((int)filter.Game) == true)
            {
                report = await reportApi.GetBetReport<BandarQiuQiuRoundResult, BandarQiuQiuBetRecord>(filter);
            }
            else
            {
                var result = await repository.GetRecordAsync(filter);
                report = new BandarQiuQiuBetReport
                {
                    RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                    Records = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetRecord>(x))
                };
            }
            if (report != null && cloudSettings?.IsCloudService != true)
            {
                report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
            }
            return report;
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            BetHistory betHistory;
            if (cloudSettings?.ShouldCallCloudService((int)filter.Game) == true)
            {
                betHistory = await reportApi.GetBetHistory(filter);
            }
            else
            {
                var result = await repository.GetBetHistoryAsync(filter);

                betHistory = new BandarQiuQiuBetHistory
                {
                    RoundResult = mapper.Map<BandarQiuQiuRoundResult>(result.Item1),
                    Bets = result.Item2.Select(x => mapper.Map<BandarQiuQiuBetHistoryRecord>(x)),
                    Odds = result.Item3,
                    CurrencyName = result.Item2.FirstOrDefault()?.Currency ?? string.Empty
                };
            }
            if (cloudSettings?.IsCloudService != true && !string.IsNullOrEmpty(betHistory?.CurrencyName))
            {
                betHistory.FractionalOddsFormat = await currencyGroupService.IsFractionalOddsFormat(filter.PartnerId, betHistory.CurrencyName);
            }
            return betHistory;
        }
    }
}