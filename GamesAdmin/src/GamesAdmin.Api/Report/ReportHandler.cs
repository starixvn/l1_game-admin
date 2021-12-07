using GamesAdmin.Api.Report.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
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
        IRequestHandler<SeDieReportRequest, SeDieBetReport>,
        IRequestHandler<KenoReportRequest, IEnumerable<KenoBetRecord>>,
        IRequestHandler<MiniSeDieReportRequest, SeDieBetReport>,
        IRequestHandler<KenoBetHistoryRequest, IEnumerable<KenoBetHistory>>,
        IRequestHandler<KenoRoundResultRequest, KenoRoundResult>,
        IRequestHandler<FishPrawnCrabReportRequest, FishPrawnCrabBetReport>,
        IRequestHandler<RouletteBetHistoryRequest, RouletteBetHistory>,
        IRequestHandler<LuckyWheelReportRequest, IEnumerable<SpinResult>>,
        IRequestHandler<SpecialSeDieReportRequest, SpecialSeDieBetReport>,
        IRequestHandler<SpecialSeDieBetHistoryRequest, SpecialSeDieBetHistory>,
        IRequestHandler<BlackjackReportRequest, IEnumerable<BlackjackBetRecord>>,
        IRequestHandler<SicBoBetHistoryRequest, SicBoBetHistory>,
        IRequestHandler<VipSeDieWarReportRequest, VipSeDieWarBetReport>,
        IRequestHandler<VipSeDieWarBetHistoryRequest, VipSeDieWarBetHistory>,
        IRequestHandler<SlotGameReportRequest, IEnumerable<SlotGameBetRecord>>,
        IRequestHandler<JackpotReportRequest, IEnumerable<JackpotRecord>>,
        IRequestHandler<LotteryBetHistoryRequest, LotteryBetHistory>,
        IRequestHandler<BitcoinMasterReportRequest, BitcoinMasterBetReport>,
        IRequestHandler<BitcoinMasterBetHistoryRequest, BitcoinMasterBetHistory>,
        IRequestHandler<KenoMatchBetHistoryRequest, KenoMatchBetHistory>,
        IRequestHandler<KenoPowerBetHistoryRequest, KenoPowerBetHistory>,
        IRequestHandler<CasinoHoldemPokerReportRequest, IEnumerable<CasinoHoldemPokerBetRecord>>,
        IRequestHandler<BetReportRequest, BetReportBase>,
       IRequestHandler<CasinoHoldemPokerBetHistoryRequest, CasinoHoldemPokerBetHistory>
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

        public async Task<SeDieBetReport> Handle(SeDieReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetSeDieReportAsync(request.Filter);

        public async Task<SeDieBetReport> Handle(MiniSeDieReportRequest request, CancellationToken cancellationToken)
            => await reportService.GetMiniSeDieReportAsync(request.Filter);

        public Task<IEnumerable<KenoBetRecord>> Handle(KenoReportRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoReportAsync(request.Game, request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<IEnumerable<KenoBetHistory>> Handle(KenoBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoBetHistoryAsync(request.Game, request.RoundId, request.Nickname);

        public Task<KenoRoundResult> Handle(KenoRoundResultRequest request, CancellationToken cancellationToken)
        => reportService.GetKenoRoundResultAsync(request.Game, request.RoundId);

        public Task<FishPrawnCrabBetReport> Handle(FishPrawnCrabReportRequest request, CancellationToken cancellationToken)
        => reportService.GetFishPrawnCrabReportAsync(request.Filter);

        public Task<RouletteBetHistory> Handle(RouletteBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetRouletteBetHistoryAsync(request.RoundId, request.UserId);

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

        public Task<VipSeDieWarBetReport> Handle(VipSeDieWarReportRequest request, CancellationToken cancellationToken)
        => reportService.GetVipSeDieWarReportAsync(request.RoundId, request.Nickname, request.ExcludeUUS);

        public Task<SicBoBetHistory> Handle(SicBoBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetSicBoBetHistoryAsync(request.RoundId, request.UserId);

        public Task<VipSeDieWarBetHistory> Handle(VipSeDieWarBetHistoryRequest request, CancellationToken cancellationToken)
        => reportService.GetVipSeDieWarBetHistoryAsync(request.RoundId, request.UserId);

        public Task<IEnumerable<JackpotRecord>> Handle(JackpotReportRequest request, CancellationToken cancellationToken)
        => reportService.GetJackpotReportAsync(request.GameId, request.FromDate, request.ToDate, request.ExcludeUUS, request.ExcludeBot);

        public Task<LotteryBetHistory> Handle(LotteryBetHistoryRequest request, CancellationToken cancellationToken)
            => reportService.GetLotteryBetHistoryAsync(request.RoundId, request.UserId);

        public async Task<BitcoinMasterBetReport> Handle(BitcoinMasterReportRequest request, CancellationToken cancellationToken)
               => await reportService.GetBitcoinMasterReportAsync(request.RoundId, request.Nickname, request.ExcludeBot, request.PartnerId, request.Username);

        public Task<BitcoinMasterBetHistory> Handle(BitcoinMasterBetHistoryRequest request, CancellationToken cancellationToken)
                => reportService.GetBitcoinMasterBetHistoryAsync(request.RoundId, request.Nickname);

        public Task<KenoMatchBetHistory> Handle(KenoMatchBetHistoryRequest request, CancellationToken cancellationToken)
        {
            return request.Game switch
            {
                GameId.KenoMatch1 => reportService.GetKenoMatch1BetHistoryAsync(request.RoundId, request.UserId),
                GameId.KenoMatch2 => reportService.GetKenoMatch2BetHistoryAsync(request.RoundId, request.UserId),
                _ => throw new ArgumentException(nameof(request.Game)),
            };
        }

        public Task<KenoPowerBetHistory> Handle(KenoPowerBetHistoryRequest request, CancellationToken cancellationToken)
        {
            return request.Game switch
            {
                GameId.KenoPower1 => reportService.GetKenoPower1BetHistoryAsync(request.RoundId, request.UserId),
                GameId.KenoPower2 => reportService.GetKenoPower2BetHistoryAsync(request.RoundId, request.UserId),
                _ => throw new ArgumentException(nameof(request.Game)),
            };
        }

        public Task<BetReportBase> Handle(BetReportRequest request, CancellationToken cancellationToken)
        {
            var service = reportServiceFactory.GetService(request.Filter.Game);
            return service.Get(request.Filter);
        }

        public Task<IEnumerable<CasinoHoldemPokerBetRecord>> Handle(CasinoHoldemPokerReportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return reportService.GetCasinoHoldemPokerReportAsync(request.Filter);
            }
            catch (Exception ex)
            {
                var a = ex;
                return null;
            }
        }

        public async Task<CasinoHoldemPokerBetHistory> Handle(CasinoHoldemPokerBetHistoryRequest request, CancellationToken cancellationToken)
              => await reportService.GetCasinoHoldemPokerBetHistoryAsync(request.Filter);
    }
}
