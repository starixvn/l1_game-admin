using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report
{
    public class HiLoReportService : IBetHistoryReportService
    {
        private readonly IReportApi reportApi;
        private readonly IMapper mapper;
        private readonly IAppSettings appSettings;

        public HiLoReportService(IReportApi reportApi, IMapper mapper, IAppSettings appSettings)
        {
            this.reportApi = reportApi;
            this.mapper = mapper;
            this.appSettings = appSettings;
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            var result = await reportApi.GetBetHistory<HiLoBetHistory>(filter);

            var roundResult = mapper.Map<HiLoRoundResultViewModel>(result.RoundResult);
            roundResult.Time = TimeZoneInfo.ConvertTimeFromUtc(roundResult.Time, appSettings.DefaultTimeZone);
            if (roundResult.EndTime.HasValue)
            {
                roundResult.EndTime = TimeZoneInfo.ConvertTimeFromUtc(roundResult.EndTime.Value, appSettings.DefaultTimeZone);
            }

            return new HiLoBetHistoryViewModel
            {
                RoundResult = roundResult,
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