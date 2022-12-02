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
    public class FanTanReportService : GameReportService, IBetHistoryReportService
    {
        public FanTanReportService(
            IReportApi reportApi,
            IPartnerService partnerService,
            IMapper mapper,
            IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<FanTanBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new FanTanReportResultViewModel
            {
                RoundResult = mapper.Map<FanTanRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<FanTanRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<FanTanBetHistory>(filter);

            var viewModel = new FanTanBetHistoryViewModel
            {
                RoundResult = mapper.Map<FanTanRoundResultViewModel>(result.RoundResult),
                Bet = mapper.Map<FanTanBetHistoryRecordViewModel>(result.Bet),
            };
        
            viewModel.Bet.Time = TimeZoneInfo.ConvertTimeFromUtc(viewModel.Bet.Time, appSettings.DefaultTimeZone);

            return viewModel;
        }
    }
}