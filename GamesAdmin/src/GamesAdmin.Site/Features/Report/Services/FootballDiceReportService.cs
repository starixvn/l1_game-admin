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
    public class FootballDiceReportService : GameReportService, IBetHistoryReportService
    {
        public FootballDiceReportService(IReportApi reportApi, IPartnerService partnerService, IMapper mapper, IAppSettings appSettings) : base(reportApi, partnerService, mapper, appSettings)
        {
        }

        public override async Task<ReportResultViewModel> Get(BetReportFilter filter)
        {
            var result = await reportApi.Get<FootballDiceBetReport>(filter);
            var partners = await partnerService.GetAll();

            return new FootballDiceReportResultViewModel
            {
                RoundResult = mapper.Map<FootballDiceRoundResultViewModel>(result.RoundResult),
                Records = result.Records.Select(x =>
                {
                    var recordViewModel = mapper.Map<FootballDiceRecordViewModel>(x);
                    recordViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);
                    recordViewModel.Partner = GetPartnerName(partners, x.PartnerId);

                    return recordViewModel;
                }),
                ConvertToUSD = filter.ConvertToUSD
            };
        }

        public async Task<BetHistoryViewModel> Get(BetHistoryFilter filter)
        {
            try
            {
                var result = await reportApi.GetBetHistory<FootballDiceBetHistory>(filter);
                return new FootballDiceBetHistoryViewModel
                {
                    RoundResult = mapper.Map<FootballDiceRoundResultViewModel>(result.RoundResult),
                    Bets = result.Bets.Select(x =>
                    {
                        var betViewModel = mapper.Map<FootballDiceBetHistoryRecordViewModel>(x);
                        betViewModel.Time = TimeZoneInfo.ConvertTimeFromUtc(x.Time, appSettings.DefaultTimeZone);

                        return betViewModel;
                    }),
                    Odds = result.Odds,
                    FractionalOddsFormat = result.FractionalOddsFormat
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}