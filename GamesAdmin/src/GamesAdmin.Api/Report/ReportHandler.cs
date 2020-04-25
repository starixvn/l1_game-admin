using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler : 
        IRequestHandler<BigSmallReportRequest, GameReport>, 
        IRequestHandler<GameReportRequest, IEnumerable<BaseBetHistory>>,
        IRequestHandler<BigSmallTurboReportRequest, GameReport>,
        IRequestHandler<OddEvenTurboReportRequest, GameReport>,
        IRequestHandler<OddEvenReportRequest, GameReport>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<GameReport> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<IEnumerable<BaseBetHistory>> Handle(GameReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBetHistoryReport(request.Name, request.RoundId, request.GameType);

        public async Task<GameReport> Handle(BigSmallTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<GameReport> Handle(OddEvenTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<GameReport> Handle(OddEvenReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

    }
}