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
    public class ColorGameReportService : GameReportService, IBetHistoryReportService
    {
        public ColorGameReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {           
            var result = await reportApi.Get<ColorGameBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new ColorGameReportResultViewModel
            {
                RoundResult = mapper.Map<ColorGameRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<ColorGameRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };            
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<ColorGameBetHistory>(filter);
            return new ColorGameBetHistoryViewModel
            {
                RoundResult = mapper.Map<ColorGameRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<ColorGameBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = result.Odds,
                FractionalOddsFormat = result.FractionalOddsFormat
            };
        }
    }
}