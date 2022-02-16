using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report
{
    public class LotteryReportService : GameReportService, IBetHistoryReportService
    {
        public LotteryReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<LotteryBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new LotteryReportResultViewModel
            {
                RoundResult = mapper.Map<LotteryRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<LotteryRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<LotteryBetHistory>(filter);

            return new LotteryBetHistoryViewModel
            {
                RoundResult = mapper.Map<LotteryRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<LotteryBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = mapper.Map<LotteryOddsViewModel>(result.Odds),
            };
        }
    }
}