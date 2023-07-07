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
    public class DragonTigerFightReportService : GameReportService, IBetHistoryReportService
    {
        public DragonTigerFightReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<DragonTigerFightBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new DragonTigerFightReportResultViewModel
            {
                RoundResult = mapper.Map<DragonTigerFightRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<DragonTigerFightRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<DragonTigerFightBetHistory>(filter);
            return new DragonTigerFightBetHistoryViewModel
            {
                RoundResult = mapper.Map<DragonTigerFightRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<DragonTigerFightBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
                Odds = result.Odds,
                FractionalOddsFormat = result.FractionalOddsFormat
            };
        }
    }
}