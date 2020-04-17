using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler : IRequestHandler<BigSmallReportRequest, BigSmallReport>, IRequestHandler<GameReportRequest, IList<Bet>>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<BigSmallReport> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<IList<Bet>> Handle(GameReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetGameReport(request.Name, request.RoundId, request.GameType, request.From, request.To);
    }
}