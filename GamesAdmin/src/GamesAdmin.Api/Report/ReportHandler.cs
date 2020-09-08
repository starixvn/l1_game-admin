using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Report.Request;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.Spins;
using MediatR;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler :
        IRequestHandler<BigSmallReportRequest, BigSmallBetReport>,
        IRequestHandler<GameReportRequest, IEnumerable<BaseBetHistory>>,
        IRequestHandler<BigSmallTurboReportRequest, BigSmallBetReport>,
        IRequestHandler<OddEvenTurboReportRequest, OddEvenBetReport>,
        IRequestHandler<OddEvenReportRequest, OddEvenBetReport>,
        IRequestHandler<SeDieReportRequest, SeDieBetReport>,
        IRequestHandler<KenoReportRequest, IEnumerable<KenoBetRecord>>,
        IRequestHandler<MiniSeDieReportRequest, SeDieBetReport>,
        IRequestHandler<KenoBetHistoryRequest, IEnumerable<KenoBetHistory>>,
        IRequestHandler<KenoRoundResultRequest, KenoRoundResult>,
        IRequestHandler<FishPrawnCrabReportRequest, FishPrawnCrabBetReport>,
        IRequestHandler<DragonTigerReportRequest, DragonTigerBetReport>,
        IRequestHandler<BaccaratReportRequest, BaccaratBetReport>,
        IRequestHandler<RouletteReportRequest, RouletteBetReport>,
        IRequestHandler<RouletteBetHistoryRequest, RouletteBetHistory>,
        IRequestHandler<LuckyWheelReportRequest, IEnumerable<SpinResult>>
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
            => await reportService.GetSeDieReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public async Task<SeDieBetReport> Handle(MiniSeDieReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetMiniSeDieReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<IEnumerable<KenoBetRecord>> Handle(KenoReportRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoReportAsync(request.Game, request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<IEnumerable<KenoBetHistory>> Handle(KenoBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoBetHistoryAsync(request.Game, request.RoundId, request.Nickname);

        public Task<KenoRoundResult> Handle(KenoRoundResultRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoRoundResultAsync(request.Game, request.RoundId);

        public Task<FishPrawnCrabBetReport> Handle(FishPrawnCrabReportRequest request, CancellationToken cancellationToken)
        => reportService.GetFishPrawnCrabReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<DragonTigerBetReport> Handle(DragonTigerReportRequest request, CancellationToken cancellationToken)
        => reportService.GetDragonTigerReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<BaccaratBetReport> Handle(BaccaratReportRequest request, CancellationToken cancellationToken)
        => reportService.GetBaccaratReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<RouletteBetReport> Handle(RouletteReportRequest request, CancellationToken cancellationToken)
        => reportService.GetRouletteReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<RouletteBetHistory> Handle(RouletteBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetRouletteBetHistoryAsync(request.RoundId, request.UserId);

        public Task<IEnumerable<SpinResult>> Handle(LuckyWheelReportRequest request, CancellationToken cancellationToken)
        => reportService.GetLuckyWheelReportAsync(request.Nickname);
    }
}