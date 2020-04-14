using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler : IRequestHandler<BigSmallReportRequest, BigSmallReport>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<BigSmallReport> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);
    }
}