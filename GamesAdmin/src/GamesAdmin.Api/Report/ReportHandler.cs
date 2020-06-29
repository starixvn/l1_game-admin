using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler :
        IRequestHandler<BigSmallReportRequest, BigSmallBetReport>,
        IRequestHandler<GameReportRequest, IEnumerable<BaseBetHistory>>,
        IRequestHandler<BigSmallTurboReportRequest, BigSmallBetReport>,
        IRequestHandler<OddEvenTurboReportRequest, OddEvenBetReport>,
        IRequestHandler<OddEvenReportRequest, OddEvenBetReport>,
        IRequestHandler<SeDieReportRequest, SeDieBetReport>,
        IRequestHandler<MiniSeDieReportRequest, SeDieBetReport>,
        IRequestHandler<KenoProVietRequest, KenoBetReport>
    {
        private readonly IReportService reportService;

        public ReportHandler(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<BigSmallBetReport> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<IEnumerable<BaseBetHistory>> Handle(GameReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBetHistoryReport(request.Name, request.RoundId, request.GameType);

        public async Task<BigSmallBetReport> Handle(BigSmallTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<OddEvenBetReport> Handle(OddEvenTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<OddEvenBetReport> Handle(OddEvenReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<SeDieBetReport> Handle(SeDieReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetSeDieReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public async Task<SeDieBetReport> Handle(MiniSeDieReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetMiniSeDieReportAsync(request.RoundId, request.Nickname, request.ExcludeBot);

        public Task<KenoBetReport> Handle(KenoReportRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoReportAsync(request.Game, request.RoundId);
    }
}