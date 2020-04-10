using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler : IRequestHandler<BigSmallReportRequest, IEnumerable<BigSmallRecord>>
    {

        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public Task<IEnumerable<BigSmallRecord>> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
        {
            var result =  reportService.GetBigSmallReport(request.RoundId, request.Nickname, request.ExcludeBot);

            return Task.FromResult(result.Select(x => new BigSmallRecord
            {
                Nickname = x.Nickname,
                Username = x.Username,
                SelectBig = x.SelectBig,
                Bet = x.Bet,
                BetWin = x.BetWin,
                Back = x.Back
            }));
        }
    }
}