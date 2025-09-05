using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report
{
    public class MiniKenoReportService : GameReportService, IBetHistoryReportService
    {
        public MiniKenoReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<MiniKenoBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new KenoReportViewModel(
                (GameId)filter.Game,
                result.RoundResult?.Number ?? 0,
                result.RoundResult,
                 result.Records.Select(x =>
                 {
                     var recordViewModel = mapper.Map<KenoRecordViewModel>(x);
                     recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                     recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                     return recordViewModel;
                 }),
                appSettings.DefaultTimeZone)
            {
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<KenoBetHistory>(filter);

            return new KenoBetHistoryViewModel
            {
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<KenoBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                }),
            };
        }
    }
}