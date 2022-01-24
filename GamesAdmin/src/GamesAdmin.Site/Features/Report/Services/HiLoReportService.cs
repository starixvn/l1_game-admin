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
    public class HiLoReportService : IBetHistoryReportService
    {
        private readonly IReportApi reportApi;
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;

        public HiLoReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings)
        {
            this.reportApi = reportApi;
            this.mapper = mapper;
            this.appSettings = appSettings;
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<HiLoBetHistory>(filter);


            return new HiLoBetHistoryViewModel
            {
                RoundResult = mapper.Map<HiLoRoundResultViewModel>(result.RoundResult),
                Bets = result.Bets.Select(x =>
                {
                    var betViewModel = mapper.Map<HiLoBetHistoryRecordViewModel>(x);
                    betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                    return betViewModel;
                })
            };
        }
    }
}