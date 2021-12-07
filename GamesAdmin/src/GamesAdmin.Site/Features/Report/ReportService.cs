using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Site.Features.Report.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Report
{
    public interface IReportService
    {
        Task<BigSmallBetReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BigSmallBetReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<OddEvenBetReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<OddEvenBetReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname);

        Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname);

        Task<IEnumerable<KenoBetHistoryRecord>> GetKenoBetHistory(Game game, long roundId, string nickname);

        Task<KenoRoundResult> GetKenoRoundResult(Game game, long roundId);

        Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId);

        Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname);

        Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId);

        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter);

        Task<SicBoBetHistory> GetSicBoBetHistory(long roundId, string userId);

        Task<VipSeDieWarBetHistory> GetVipSeDieWarBetHistory(long roundId, string userId);

        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request);

        Task<IEnumerable<JackpotRecord>> GetJackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);

        Task<LotteryBetHistory> GetLotteryBetHistory(long roundId, string userId);

        Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname);

        Task<KenoMatchBetHistory> GetKenoMatchBetHistory(GameId game, long roundId, string userId);

        Task<KenoPowerBetHistory> GetKenoPowerBetHistory(GameId game, long roundId, string userId);

        Task<IEnumerable<CasinoHoldemPokerBetRecord>> GetCasinoHoldemPokerBetReport(CasinoHoldemPokerBetReportFilter filter);

        Task<CasinoHoldemPokerBetHistory> GetCasinoHoldemPokerBetHistory(CasinoHoldemPokerBetHistoryFilter filter);
    }

    public class ReportService : IReportService
    {
        private readonly IReportApi reportApi;

        public ReportService(IReportApi reportApi)
        {
            this.reportApi = reportApi;
        }

        public async Task<BigSmallBetReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => await reportApi.GetBigSmallReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<BigSmallBetReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => await reportApi.GetBigSmallTurboReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<OddEvenBetReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => await reportApi.GetOddEvenReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<OddEvenBetReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => await reportApi.GetOddEvenTurboReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmall, roundId, nickname);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmallTurbo, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.OddEven, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.OddEvenTurbo, roundId, nickname);

        public Task<IEnumerable<KenoBetHistoryRecord>> GetKenoBetHistory(Game game, long roundId, string nickname)
        => reportApi.GetKenoBetHistory(game.Value, roundId, nickname);

        public Task<KenoRoundResult> GetKenoRoundResult(Game game, long roundId)
        => reportApi.GetKenoRoundResult(game.Value, roundId);

        public async Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId)
        => await reportApi.GetRouletteBetHistory(roundId, userId);

        public Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname)
        => reportApi.GetLuckyWheelBetHistory(nickname);

        public Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => reportApi.GetSpecialSeDieReport(roundId, nickname, excludeBot, partnerId, username);

        public Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId)
        => reportApi.GetSpecialSeDieBetHistory(roundId, userId);

        public Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter)
        => reportApi.GetBlackjackReport(filter);

        public async Task<VipSeDieWarBetHistory> GetVipSeDieWarBetHistory(long roundId, string userId)
            => await reportApi.GetVipSeDieWarBetHistory(roundId, userId);

        public Task<SicBoBetHistory> GetSicBoBetHistory(long roundId, string userId)
        => reportApi.GetSicBoBetHistory(roundId, userId);

        public Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request)
            => reportApi.GetSlotGameReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeUUS,
                request.Game.Value,
                request.PartnerId,
                request.FromDate,
                request.ToDate,
                request.Username,
                request.ConvertToUSD);

        public Task<IEnumerable<JackpotRecord>> GetJackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot)
            => reportApi.GetJackpotReport(gameId, fromDate, toDate, excludeUUS, excludeBot);

        public Task<LotteryBetHistory> GetLotteryBetHistory(long roundId, string userId)
            => reportApi.GetLotteryBetHistory(roundId, userId);

        public async Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => await reportApi.GetBitcoinMasterReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname)
            => await reportApi.GetBitcoinMasterBetReport(roundId, nickname);

        public Task<KenoMatchBetHistory> GetKenoMatchBetHistory(GameId game, long roundId, string userId)
        => reportApi.GetKenoMatchBetHistory(game.ToString(), roundId, userId);

        public Task<KenoPowerBetHistory> GetKenoPowerBetHistory(GameId game, long roundId, string userId)
        => reportApi.GetKenoPowerBetHistory(game.ToString(), roundId, userId);

        public Task<IEnumerable<CasinoHoldemPokerBetRecord>> GetCasinoHoldemPokerBetReport(CasinoHoldemPokerBetReportFilter filter)
            => reportApi.GetCasinoHoldemPokerBetReport(filter);

        public async Task<CasinoHoldemPokerBetHistory> GetCasinoHoldemPokerBetHistory(CasinoHoldemPokerBetHistoryFilter filter)
            => await reportApi.GetCasinoHoldemPokerBetHistory(filter);
    }
}
