using AutoMapper;
using GamesAdmin.Api.UserInfo;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Report;
using System.Threading.Tasks;
using System;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api._Shared.Configurations;
using System.Linq;
using GamesAdmin.Database.Entities;
using GamesAdmin.Api.Report.Services;

namespace GamesAdmin.Api.Report
{
    public class NextBallReportService<TRound, TBet, TBetLog> : BaseReportService, IGameReportService, IBetHistoryReportService
             where TRound : BaseRoundEntity, new() where TBet : BaseBetEntity where TBetLog : BaseBetLogEntity

    {
        private readonly IBaseDrawGameReportRepository repository;
        private readonly IMapper mapper;

        public NextBallReportService(IBaseDrawGameReportRepository repository,
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
            if (cloudSettings?.ShouldCallCloudService(filter.Game) == true)
            {
                var header = await IdentityServerManager.GetAuthorizationHeader();
                var report = await reportApi.GetBetReport<NextBallBetReport>(filter, header);
                report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
                return report;
            }
            else
            {
                var result = await repository.GetRecordAsync<TRound, TBet>(filter);
                var report = new NextBallBetReport
                {
                    RoundResult = mapper.Map<NextBallRound>(result.Item1),
                    Records = result.Item2.Select(x => mapper.Map<NextBallBetRecord>(x))
                };
                if (!cloudSettings.IsCloudGame(filter.Game))
                {
                    report.Records = await GetBetRecordsWithUserInfo(report.Records, filter.Username, filter.ExcludeBot);
                }
                return report;
            }
        }

        public async Task<BetHistory> Get(BetHistoryFilter filter)
        {
            if (cloudSettings?.ShouldCallCloudService(filter.Game) == true)
            {
                return await GetBetHistoryOnCloud<NextBallBetHistory>(filter);
            }
            else
            {
                var result = await repository.GetBetHistoryAsync<TRound, TBet, TBetLog>(filter);

                var betHistory = new NextBallBetHistory
                {
                    RoundResult = mapper.Map<NextBallRound>(result.Item1),
                    Bets = result.Item2.Select(x => mapper.Map<NextBallBetHistoryRecord>(x)),
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
}