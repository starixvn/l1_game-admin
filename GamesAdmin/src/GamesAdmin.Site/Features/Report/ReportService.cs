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

        Task<SeDieBetReport> GetSeDieReport(BetReportFilter filter);

        Task<SeDieBetReport> GetMiniSeDieReport(BetReportFilter filter);

        Task<IEnumerable<KenoBetRecord>> GetKenoBetReport(Game game, long roundId, string nickname, bool excludeUUS);

        Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname);

        Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname);

        Task<IEnumerable<KenoBetHistory>> GetKenoBetHistory(Game game, long roundId, string nickname);

        Task<KenoRoundResult> GetKenoRoundResult(Game game, long roundId);

        Task<BaccaratBetReport> GetBaccaratBetReport(long roundId, string nickname, bool excludeUUS);

        Task<VipBaccaratBetReport> GetVipBaccaratBetReport(long roundId, string nickname, bool excludeUUS);

        Task<RouletteBetReport> GetRouletteBetReport(long roundId, string nickname, bool excludeUUS);

        Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId);

        Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname);

        Task<VipSeDieBetReport> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS);

        Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId);

        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter);

        Task<VipFishPrawnCrabBetReport> GetVipFishPrawnCrabBetReport(long roundId, string nickname, bool excludeUUS);

        Task<SicBoBetReport> GetSicBoBetReport(long roundId, string nickname, bool excludeUUS);

        Task<VipSeDieWarBetReport> GetVipSeDieWarBetReport(long roundId, string nickname, bool excludeUUS);

        Task<SicBoBetHistory> GetSicBoBetHistory(long roundId, string userId);

        Task<VipSeDieWarBetHistory> GetVipSeDieWarBetHistory(long roundId, string userId);

        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request);

        Task<IEnumerable<JackpotRecord>> GetJackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);

        Task<LotteryBetReport> GetLotteryBetReport(long roundId, string nickname, bool excludeUUS);

        Task<LotteryBetHistory> GetLotteryBetHistory(long roundId, string userId);

        Task<ThreeCardsBetReport> GetThreeCardsBetReport(long roundId, string nickname, bool excludeUUS);

        Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname);

        Task<KenoMatchBetReport> GetKenoMatchBetReport(GameId game, long roundId, string nickname, bool excludeUUS);

        Task<KenoMatchBetHistory> GetKenoMatchBetHistory(GameId game, long roundId, string userId);

        Task<KenoPowerBetReport> GetKenoPowerBetReport(GameId game, long roundId, string nickname, bool excludeUUS);

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

        public async Task<SeDieBetReport> GetSeDieReport(BetReportFilter filter)
            => await reportApi.GetSeDieReport(filter);

        public async Task<SeDieBetReport> GetMiniSeDieReport(BetReportFilter filter)
            => await reportApi.GetMiniSeDieReport(filter);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmall, roundId, nickname);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmallTurbo, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.OddEven, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.OddEvenTurbo, roundId, nickname);

        public Task<IEnumerable<KenoBetRecord>> GetKenoBetReport(Game game, long roundId, string nickname, bool excludeUUS)
        => reportApi.GetKenoReport(game.Value, roundId, nickname, excludeUUS);

        public Task<IEnumerable<KenoBetHistory>> GetKenoBetHistory(Game game, long roundId, string nickname)
        => reportApi.GetKenoBetHistory(game.Value, roundId, nickname);

        public Task<KenoRoundResult> GetKenoRoundResult(Game game, long roundId)
        => reportApi.GetKenoRoundResult(game.Value, roundId);

        public async Task<BaccaratBetReport> GetBaccaratBetReport(long roundId, string nickname, bool excludeUUS)
        => await reportApi.GetBaccaratReport(roundId, nickname, excludeUUS);

        public async Task<RouletteBetReport> GetRouletteBetReport(long roundId, string nickname, bool excludeUUS)
        => await reportApi.GetRouletteReport(roundId, nickname, excludeUUS);

        public async Task<RouletteBetHistory> GetRouletteBetHistory(long roundId, string userId)
        => await reportApi.GetRouletteBetHistory(roundId, userId);

        public Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname)
        => reportApi.GetLuckyWheelBetHistory(nickname);

        public Task<VipSeDieBetReport> GetVipSeDieReport(long roundId, string nickname, bool excludeUUS)
        => reportApi.GetVipSeDieReport(roundId, nickname, excludeUUS);

        public Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => reportApi.GetSpecialSeDieReport(roundId, nickname, excludeBot, partnerId, username);

        public Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId)
        => reportApi.GetSpecialSeDieBetHistory(roundId, userId);

        public Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter)
        => reportApi.GetBlackjackReport(filter);

        public Task<VipFishPrawnCrabBetReport> GetVipFishPrawnCrabBetReport(long roundId, string nickname, bool excludeUUS)
        => reportApi.GetVipFishPrawnCrabReport(roundId, nickname, excludeUUS);

        public Task<VipSeDieWarBetReport> GetVipSeDieWarBetReport(long roundId, string nickname, bool excludeUUS)
            => reportApi.GetVipSeDieWarReport(roundId, nickname, excludeUUS);

        public async Task<VipSeDieWarBetHistory> GetVipSeDieWarBetHistory(long roundId, string userId)
            => await reportApi.GetVipSeDieWarBetHistory(roundId, userId);

        public Task<VipBaccaratBetReport> GetVipBaccaratBetReport(long roundId, string nickname, bool excludeUUS)
        => reportApi.GetVipBaccaratReport(roundId, nickname, excludeUUS);

        public Task<SicBoBetReport> GetSicBoBetReport(long roundId, string nickname, bool excludeUUS)
        => reportApi.GetSicBoBetReport(roundId, nickname, excludeUUS);

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

        public Task<LotteryBetReport> GetLotteryBetReport(long roundId, string nickname, bool excludeUUS)
            => reportApi.GetLotteryBetReport(roundId, nickname, excludeUUS);

        public Task<LotteryBetHistory> GetLotteryBetHistory(long roundId, string userId)
            => reportApi.GetLotteryBetHistory(roundId, userId);

        public Task<ThreeCardsBetReport> GetThreeCardsBetReport(long roundId, string nickname, bool excludeUUS)
            => reportApi.GetThreeCardsBetReport(roundId, nickname, excludeUUS);

        public async Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => await reportApi.GetBitcoinMasterReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname)
            => await reportApi.GetBitcoinMasterBetReport(roundId, nickname);

        public Task<KenoMatchBetReport> GetKenoMatchBetReport(GameId game, long roundId, string nickname, bool excludeUUS)
          => reportApi.GetKenoMatchBetReport(game.ToString(), roundId, nickname, excludeUUS);

        public Task<KenoMatchBetHistory> GetKenoMatchBetHistory(GameId game, long roundId, string userId)
        => reportApi.GetKenoMatchBetHistory(game.ToString(), roundId, userId);

        public Task<KenoPowerBetReport> GetKenoPowerBetReport(GameId game, long roundId, string nickname, bool excludeUUS)
        => reportApi.GetKenoPowerBetReport(game.ToString(), roundId, nickname, excludeUUS);

        public Task<KenoPowerBetHistory> GetKenoPowerBetHistory(GameId game, long roundId, string userId)
        => reportApi.GetKenoPowerBetHistory(game.ToString(), roundId, userId);

        public Task<IEnumerable<CasinoHoldemPokerBetRecord>> GetCasinoHoldemPokerBetReport(CasinoHoldemPokerBetReportFilter filter)
            => reportApi.GetCasinoHoldemPokerBetReport(filter);

        public async Task<CasinoHoldemPokerBetHistory> GetCasinoHoldemPokerBetHistory(CasinoHoldemPokerBetHistoryFilter filter)
            => await reportApi.GetCasinoHoldemPokerBetHistory(filter);
    }
}
