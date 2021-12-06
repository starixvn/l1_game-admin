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
    public class KenoMatchReportService : GameReportService
    {
        public KenoMatchReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<KenoMatchBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new KenoMatchReportResultViewModel
            {
                Game = filter.Game,
                RoundResult = mapper.Map<KenoMatchRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<KenoMatchRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }
    }
}