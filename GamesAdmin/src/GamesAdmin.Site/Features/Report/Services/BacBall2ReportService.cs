using System;
using System.Linq;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Report.ViewModels;
using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Partners;

namespace GamesAdmin.Site.Features.Report
{
    public class BacBall2ReportService : GameReportService, IBetHistoryReportService
    {
        public BacBall2ReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<BacBall2BetReport>(filter);
            var partners = await partnerService.GetAll();
            var betRecords = result.Records.GroupBy(r => r.UserId, r => r, (key, g) => new BacBall2BetRecord
            {
                Time = g.First().Time,
                PartnerId  = g.First().PartnerId,                
                Username = g.First().Username,
                Nickname = g.First().Nickname,
                Currency = g.First().Currency,
                TotalAmount = g.Sum(r => r.TotalAmount),
                BetWin = g.Sum(r => r.BetWin),
                UserId = g.First().UserId,
                DrawId = g.First().DrawId,
                Rate = g.First().Rate
            });

            return new BacBall2ReportResultViewModel
            {
                DrawId = filter.RoundId,
                RoundResult = result.RoundResult,
                Records = betRecords.Select(x =>
                {
                    var recordViewModel = mapper.Map<BacBall2RecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<BacBall2BetHistory>(filter);
            return new BacBall2BetHistoryViewModel
            {
                RoundResult = result.RoundResult,
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<BacBall2BetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = result.Odds,
                FractionalOddsFormat = result.FractionalOddsFormat
            };
        }
    }
}