using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ReportHandler :
        IRequestHandler<GetBigSmallReportRequest, BigSmallReportResultViewModel>,
        IRequestHandler<GetBigSmallTurboReportRequest, BigSmallTurboReportResultViewModel>,
        IRequestHandler<GetOddEvenReportRequest, OddEvenReportResultViewModel>,
        IRequestHandler<GetOddEvenTurboReportRequest, OddEvenTurboReportResultViewModel>
    {
        private readonly IReportService reportService;
        private readonly IAppSettings appSettings;

        public ReportHandler(IReportService reportService, IAppSettings appSettings)
        {
            this.reportService = reportService;
            this.appSettings = appSettings;
        }

        public async Task<BigSmallReportResultViewModel> Handle(GetBigSmallReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new BigSmallReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new RecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTime(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<BigSmallTurboReportResultViewModel> Handle(GetBigSmallTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallTurboReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new BigSmallTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new RecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTime(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<OddEvenReportResultViewModel> Handle(GetOddEvenReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEvenReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new OddEvenReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new RecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTime(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }

        public async Task<OddEvenTurboReportResultViewModel> Handle(GetOddEvenTurboReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetOddEventTurboReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return new OddEvenTurboReportResultViewModel
            {
                RoundResult = new RoundResultViewModel
                {
                    Number = result.RoundResult.Number,
                    Dice1 = result.RoundResult.Dice1,
                    Dice2 = result.RoundResult.Dice2,
                    Dice3 = result.RoundResult.Dice3,
                },
                Records = result.Records.Select(x => new RecordViewModel
                {
                    Time = TimeZoneInfo.ConvertTime(x.Time, appSettings.DefaultTimeZone),
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    Select = x.Select
                })
            };
        }
    }
}