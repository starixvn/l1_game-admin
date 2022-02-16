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
    public class FishPrawnCrabReportService : GameReportService
    {
        public FishPrawnCrabReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<FishPrawnCrabBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new FishPrawnCrabReportResultViewModel
            {
                RoundResult = mapper.Map<FishPrawnCrabRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<FishPrawnCrabRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }
    }
}