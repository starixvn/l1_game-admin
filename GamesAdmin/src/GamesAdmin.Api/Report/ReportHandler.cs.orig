using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        IRequestHandler<LuckyWheelReportRequest, IEnumerable<SpinResult>>,
        IRequestHandler<SpecialSeDieReportRequest, SpecialSeDieBetReport>,
        IRequestHandler<SpecialSeDieBetHistoryRequest, SpecialSeDieBetHistory>,
        IRequestHandler<BlackjackReportRequest, IEnumerable<BlackjackBetRecord>>,
        IRequestHandler<SlotGameReportRequest, IEnumerable<SlotGameBetRecord>>,
        IRequestHandler<JackpotReportRequest, IEnumerable<JackpotRecord>>,
        IRequestHandler<BitcoinMasterReportRequest, BitcoinMasterBetReport>,
        IRequestHandler<BitcoinMasterBetHistoryRequest, BitcoinMasterBetHistory>,
        IRequestHandler<BetReportRequest, BetReportBase>,
        IRequestHandler<BetHistoryRequest, BetHistory>,
        IRequestHandler<HoldemPokerReportRequest, IEnumerable<HoldemPokerBetRecord>>,
        IRequestHandler<HoldemPokerBetHistoryRequest, HoldemPokerBetHistory>
        IRequestHandler<HiLoReportRequest, IEnumerable<HiLoBetRecord>>
    {
        private readonly IReportService reportService;
        private readonly IReportServiceFactory reportServiceFactory;
        private readonly IServiceProvider serviceProvider;

        public ReportHandler(IReportService reportService, IReportServiceFactory reportServiceFactory, IServiceProvider serviceProvider)
        {
            this.reportService = reportService;
            this.reportServiceFactory = reportServiceFactory;
            this.serviceProvider = serviceProvider;
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
    }
}
