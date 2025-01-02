using AutoMapper;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.Report.Services;
using GamesAdmin.Api.UserInfo;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Report;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class BaccaratNumberReportService<TRound, TBet, TBetLog> : BaseReportService, IGameReportService, IBetHistoryReportService
        where TRound : BacBall1RoundEntity, new()
        where TBet : BacBall1BetEntity
        where TBetLog : BacBall1BetLogEntity
    {
        private readonly IBaccaratNumberReportRepository repository;
        private readonly IMapper mapper;

        public BaccaratNumberReportService(IBaccaratNumberReportRepository repository,
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
            var result = await repository.GetRecordAsync<TRound, TBet>(filter);
            var report = new BacBall1BetReport
            {
                RoundResult = mapper.Map<BacBall1Round>(result.Item1),
                Records = result.Item2.Select(x => mapper.Map<BacBall1BetRecord>(x))
            };

            report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
            return report;
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            var result = await repository.GetBetHistoryAsync<TRound, TBet, TBetLog>(filter);

            var betHistory = new BacBall1BetHistory
            {
                RoundResult = mapper.Map<BacBall1Round>(result.Item1),
                Bets = result.Item2.Select(x => mapper.Map<BacBall1BetHistoryRecord>(x)),
                Odds = result.Item3,
                CurrencyName = result.Item2.FirstOrDefault()?.Currency ?? string.Empty
            };
            if (!cloudSettings.IsCloudGame((int)filter.Game))
            {
                betHistory.FractionalOddsFormat = await currencyGroupService.IsFractionalOddsFormat(filter.PartnerId, betHistory.CurrencyName);
            }
            return betHistory;
        }
    }
}