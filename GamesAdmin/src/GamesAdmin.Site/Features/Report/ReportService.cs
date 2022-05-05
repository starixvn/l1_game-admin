using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Site.Features.Report.Requests;

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

        Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname);

        Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId);

        Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter);

        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request);

        Task<IEnumerable<JackpotRecord>> GetJackpotReport(byte gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot);

        Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname);

        Task<IEnumerable<HiLoBetRecord>> GetHiLoReport(HiLoBetReportFilter filter);

        Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerBetReport(HoldemPokerBetReportFilter filter);

        Task<HoldemPokerBetHistory> GetHoldemPokerBetHistory(HoldemPokerBetHistoryFilter filter);
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

        public Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname)
        => reportApi.GetLuckyWheelBetHistory(nickname);

        public Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => reportApi.GetSpecialSeDieReport(roundId, nickname, excludeBot, partnerId, username);

        public Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId)
        => reportApi.GetSpecialSeDieBetHistory(roundId, userId);

        public Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter)
        => reportApi.GetBlackjackReport(filter);

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

        public async Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
            => await reportApi.GetBitcoinMasterReport(roundId, nickname, excludeBot, partnerId, username);

        public async Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname)
            => await reportApi.GetBitcoinMasterBetReport(roundId, nickname);


        public Task<IEnumerable<HiLoBetRecord>> GetHiLoReport(HiLoBetReportFilter filter)
        => reportApi.GetHiLoReport(filter);

        public Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerBetReport(HoldemPokerBetReportFilter filter)
            => reportApi.GetHoldemPokerBetReport(filter);

        public async Task<HoldemPokerBetHistory> GetHoldemPokerBetHistory(HoldemPokerBetHistoryFilter filter)
            => await reportApi.GetHoldemPokerBetHistory(filter);
    }
}
