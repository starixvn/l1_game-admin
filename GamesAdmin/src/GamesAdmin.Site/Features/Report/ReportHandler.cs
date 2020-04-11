using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.Report.Requests;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ReportHandler :
        IRequestHandler<GetBigSmallReportRequest, IEnumerable<BigSmallRecordViewModel>>,
        IRequestHandler<GetBigSmallRoundsRequest, IEnumerable<long>>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<IEnumerable<BigSmallRecordViewModel>> Handle(GetBigSmallReportRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return result.Select(x => new BigSmallRecordViewModel
            {
                Username = x.Username,
                Nickname = x.Nickname,
                Back = x.Back,
                Bet = x.Bet,
                BetWin = x.BetWin,
                SelectBig = x.SelectBig
            });
        }

        public async Task<IEnumerable<long>> Handle(GetBigSmallRoundsRequest request, CancellationToken cancellationToken)
        {
            var result = await reportService.GetBigSmallRounds();

            return result.OrderByDescending(x => x.Number).Select(x => x.Number);
        }
    }
}