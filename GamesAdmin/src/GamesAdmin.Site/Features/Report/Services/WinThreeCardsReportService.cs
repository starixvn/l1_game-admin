using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Report
{
    public class WinThreeCardsReportService : GameReportService, IBetHistoryReportService
    {
        public WinThreeCardsReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<WinThreeCardsBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new WinThreeCardsReportResultViewModel
            {
                RoundResult = mapper.Map<WinThreeCardsRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<WinThreeCardsRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<WinThreeCardsBetHistory>(filter);

            var betViewModel = mapper.Map<WinThreeCardsBetHistoryRecordViewModel>(result.Bet);
            betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(result.Bet.Time, appSettings.DefaultTimeZone);

            return new WinThreeCardsBetHistoryViewModel
            {
                RoundResult = mapper.Map<WinThreeCardsRoundResultViewModel>(result.RoundResult),
                Bet = betViewModel,
                Odds = result.Odds
            };
        }
    }
}