using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ReportHandler :
        IRequestHandler<GetBigSmallReportRequest, BigSmallReportResultViewModel>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
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
                Records = result.Records.Select(x => new BigSmallRecordViewModel
                {
                    Time = x.Time,
                    Username = x.Username,
                    Nickname = x.Nickname,
                    Back = x.Back,
                    Bet = x.Bet,
                    BetWin = x.BetWin,
                    SelectBig = x.SelectBig
                })
            };
        }
    }
}