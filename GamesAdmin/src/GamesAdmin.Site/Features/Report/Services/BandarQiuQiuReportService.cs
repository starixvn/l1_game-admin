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
    public class BandarQiuQiuReportService : GameReportService, IBetHistoryReportService
    {
        public BandarQiuQiuReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<BandarQiuQiuBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new BandarQiuQiuReportResultViewModel
            {
                RoundResult = mapper.Map<BandarQiuQiuRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<BandarQiuQiuRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<BandarQiuQiuBetHistory>(filter);
            return new BandarQiuQiuBetHistoryViewModel
            {
                RoundResult = mapper.Map<BandarQiuQiuRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<BandarQiuQiuBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = result.Odds,
                FractionalOddsFormat = result.FractionalOddsFormat
            };
        }
    }
}