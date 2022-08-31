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
    public class FruitSlotReportService : GameReportService, IBetHistoryReportService
    {
        public FruitSlotReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<FruitSlotBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new FruitSlotReportResultViewModel
            {
                RoundResult = mapper.Map<FruitSlotRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<FruitSlotRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<FruitSlotBetHistory>(filter);

            return new FruitSlotBetHistoryViewModel
            {
                RoundResult = mapper.Map<FruitSlotRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<FruitSlotBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = result.Odds
            };
        }
    }
}