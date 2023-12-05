using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Api.Report.Services;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api.Report
{
    public class ReportHandler :
        IRequestHandler<BigSmallReportRequest, BigSmallBetReport>,
        IRequestHandler<GameReportRequest, IEnumerable<BaseBetHistory>>,
        IRequestHandler<BigSmallTurboReportRequest, BigSmallBetReport>,
        IRequestHandler<OddEvenTurboReportRequest, OddEvenBetReport>,
        IRequestHandler<OddEvenReportRequest, OddEvenBetReport>,
        IRequestHandler<LuckyWheelReportRequest, IEnumerable<SpinResult>>,
        IRequestHandler<SpecialSeDieReportRequest, SpecialSeDieBetReport>,
        IRequestHandler<SpecialSeDieBetHistoryRequest, SpecialSeDieBetHistory>,
        IRequestHandler<BlackjackReportRequest, IEnumerable<BlackjackBetRecord>>,
        IRequestHandler<PontoonReportRequest, IEnumerable<BlackjackBetRecord>>,
        IRequestHandler<SlotGameReportRequest, IEnumerable<SlotGameBetRecord>>,
        IRequestHandler<JackpotReportRequest, IEnumerable<JackpotRecord>>,
        IRequestHandler<BitcoinMasterReportRequest, BitcoinMasterBetReport>,
        IRequestHandler<BitcoinMasterBetHistoryRequest, BitcoinMasterBetHistory>,
        IRequestHandler<BetReportRequest, BetReportBase>,
        IRequestHandler<BetHistoryRequest, BetHistory>,
        IRequestHandler<HoldemPokerReportRequest, IEnumerable<HoldemPokerBetRecord>>,
        IRequestHandler<HoldemPokerBetHistoryRequest, HoldemPokerBetHistory>,
        IRequestHandler<HiLoReportRequest, IEnumerable<HiLoBetRecord>>,
        IRequestHandler<CascadingSlotReportRequest, IEnumerable<CascadingSlotBetRecord>>,
        IRequestHandler<CascadingSlotBetHistoryRequest, CascadingSlotBetHistory>,
        IRequestHandler<CrashGameReportRequest, CrashGameBetReport>
    {
        private readonly IReportService reportService;
        private readonly IReportServiceFactory reportServiceFactory;
        private readonly IServiceProvider serviceProvider;
        private readonly IHoldemPokerReportService holdemPokerReportService;

        public ReportHandler(
            IReportService reportService,
            IReportServiceFactory reportServiceFactory,
            IServiceProvider serviceProvider,
            IHoldemPokerReportService holdemPokerReportService)
        {
            this.reportService = reportService;
            this.reportServiceFactory = reportServiceFactory;
            this.serviceProvider = serviceProvider;
            this.holdemPokerReportService = holdemPokerReportService;
        }

        public async Task<BigSmallBetReport> Handle(BigSmallReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public async Task<IEnumerable<BaseBetHistory>> Handle(GameReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBetHistoryReport(request.Name, request.RoundId, request.GameType);

        public async Task<BigSmallBetReport> Handle(BigSmallTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetBigSmallTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public async Task<OddEvenBetReport> Handle(OddEvenTurboReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenTurboReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public async Task<OddEvenBetReport> Handle(OddEvenReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetOddEvenReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public Task<IEnumerable<SpinResult>> Handle(LuckyWheelReportRequest request, CancellationToken cancellationToken)
            => reportService.GetLuckyWheelReportAsync(request.Nickname);

        public Task<SpecialSeDieBetReport> Handle(SpecialSeDieReportRequest request, CancellationToken cancellationToken)
            => reportService.GetSpecialSeDieReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public Task<SpecialSeDieBetHistory> Handle(SpecialSeDieBetHistoryRequest request, CancellationToken cancellationToken)
            => reportService.GetSpecialSeDieBetHistoryAsync(request.RoundId, request.UserId);

        public Task<IEnumerable<BlackjackBetRecord>> Handle(BlackjackReportRequest request, CancellationToken cancellationToken)
            => serviceProvider.GetRequiredService<BlackjackReportService>().Get(request.Filter);

        public Task<IEnumerable<BlackjackBetRecord>> Handle(PontoonReportRequest request, CancellationToken cancellationToken)
           => serviceProvider.GetRequiredService<PontoonReportService>().Get(request.Filter);

        public Task<IEnumerable<SlotGameBetRecord>> Handle(SlotGameReportRequest request, CancellationToken cancellationToken)
            => reportService.GetSlotGameReportAsync(request.Filter);

        public Task<IEnumerable<JackpotRecord>> Handle(JackpotReportRequest request, CancellationToken cancellationToken)
            => reportService.GetJackpotReportAsync(request.GameId, request.FromDate, request.ToDate, request.ExcludeUUS, request.ExcludeBot);

        public async Task<BitcoinMasterBetReport> Handle(BitcoinMasterReportRequest request, CancellationToken cancellationToken)
               => await reportService.GetBitcoinMasterReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public Task<BitcoinMasterBetHistory> Handle(BitcoinMasterBetHistoryRequest request, CancellationToken cancellationToken)
                => reportService.GetBitcoinMasterBetHistoryAsync(request.RoundId, request.Nickname);

        public Task<BetReportBase> Handle(BetReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetService(request.Filter.Game);
            return service.Get(request.Filter);
        }

        public Task<BetHistory> Handle(BetHistoryRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetBetHistoryService(request.Filter.Game);
            return service.Get(request.Filter);
        }

        public async Task<IEnumerable<HoldemPokerBetRecord>> Handle(HoldemPokerReportRequest request, CancellationToken cancellationToken)
            => await holdemPokerReportService.GetHoldemPokerReportAsync(request.Filter);

        public async Task<HoldemPokerBetHistory> Handle(HoldemPokerBetHistoryRequest request, CancellationToken cancellationToken)
            => await holdemPokerReportService.GetHoldemPokerBetHistoryAsync(request.Filter);

        public Task<IEnumerable<HiLoBetRecord>> Handle(HiLoReportRequest request, CancellationToken cancellationToken)
            => serviceProvider.GetRequiredService<HiLoReportService>().Get(request.Filter);

        public Task<IEnumerable<CascadingSlotBetRecord>> Handle(CascadingSlotReportRequest request, CancellationToken cancellationToken)
            => reportService.GetCascadingSlotReportAsync(request.Filter);

        public Task<CascadingSlotBetHistory> Handle(CascadingSlotBetHistoryRequest request, CancellationToken cancellationToken)
            => reportService.GetCascadingSlotBetHistoryAsync(request.Filter);

        public Task<CrashGameBetReport> Handle(CrashGameReportRequest request, CancellationToken cancellationToken)
           => reportService.GetCrashGameReportAsync(request.Filter);
    }
}