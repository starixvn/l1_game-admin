using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.SlotGame;
using GamesAdmin.Core.Models.Spins;
using GamesAdmin.Site.Features.Partners;
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

        Task<IEnumerable<BlackjackBetRecord>> GetPontoonReport(BlackjackBetReportFilter filter);

        Task<IEnumerable<BlackjackBetRecord>> GetChineseBlackjackReport(BlackjackBetReportFilter filter);

        Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request);

        Task<IEnumerable<JackpotRecord>> GetJackpotReport(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot, string operatorId, IEnumerable<string> siteNames);

        Task<BitcoinMasterBetReport> GetBitcoinMasterReport(long roundId, string nickname, bool excludeBot, string partnerId, string username);

        Task<BitcoinMasterBetHistory> GetBitcoinMasterBetHistory(long roundId, string nickname);

        Task<IEnumerable<HiLoBetRecord>> GetHiLoReport(HiLoBetReportFilter filter);

        Task<IEnumerable<HoldemPokerBetRecord>> GetHoldemPokerBetReport(HoldemPokerBetReportFilter filter);

        Task<HoldemPokerBetHistory> GetHoldemPokerBetHistory(HoldemPokerBetHistoryFilter filter);

        Task<IEnumerable<CascadingSlotBetRecord>> GetCascadingSlotReport(CascadingSlotReportFilter filter);

        Task<CascadingSlotBetHistory> GetCascadingSlotBetHistory(CascadingSlotReportFilter filter);

        Task<CrashGameBetReport> GetCrashGameReport(CrashGameBetReportFilter filter);

        Task<IEnumerable<CrashTurnGameBetRecord>> GetCrashTurnGameReport(CrashTurnGameBetReportFilter filter);

        Task<string> GetRemoteRoundHistoryUrl(BetReportFilter request);

        Task<IEnumerable<JackpotPool>> GetJackpotRunningPools(int gameId, string operatorId, IEnumerable<string> siteNames);
    }

    public class ReportService : IReportService
    {
        private readonly IReportApi reportApi;
        private readonly IRemoteReportApi remoteReportApi;
        private readonly IPartnerService operatorService;

        public ReportService(IReportApi reportApi, IRemoteReportApi remoteReportApi, IPartnerService operatorService)
        {
            this.reportApi = reportApi;
            this.remoteReportApi = remoteReportApi;
            this.operatorService = operatorService;
        }

        public async Task<string> GetRemoteRoundHistoryUrl(BetReportFilter request)
        {
            try
            {
                var Operators = await operatorService.GetAll();
                request.Auth = new Core.Models.ApiLogs.Auth
                {
                    OperatorId = Operators.FirstOrDefault()?.PartnerId,
                    OperatorSecret = Operators.FirstOrDefault()?.SecretKey
                };

                var result = await remoteReportApi.GetAccessUrl(request);
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
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
        => await reportApi.GetBetHistory<BigSmallBet>((int)GameId.BigSmall, roundId, nickname);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((int)GameId.BigSmallTurbo, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((int)GameId.OddEven, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((int)GameId.OddEvenTurbo, roundId, nickname);

        public Task<IEnumerable<SpinResult>> GetLuckyWheelBetHistory(string nickname)
        => reportApi.GetLuckyWheelBetHistory(nickname);

        public Task<SpecialSeDieBetReport> GetSpecialSeDieReport(long roundId, string nickname, bool excludeBot, string partnerId, string username)
        => reportApi.GetSpecialSeDieReport(roundId, nickname, excludeBot, partnerId, username);

        public Task<SpecialSeDieBetHistory> GetSpecialSeDieBetHistory(long roundId, string userId)
        => reportApi.GetSpecialSeDieBetHistory(roundId, userId);

        public Task<IEnumerable<BlackjackBetRecord>> GetBlackjackReport(BlackjackBetReportFilter filter)
        => reportApi.GetBlackjackReport(filter);

        public Task<IEnumerable<BlackjackBetRecord>> GetPontoonReport(BlackjackBetReportFilter filter)
       => reportApi.GetPontoonReport(filter);

        public Task<IEnumerable<BlackjackBetRecord>> GetChineseBlackjackReport(BlackjackBetReportFilter filter)
            => reportApi.GetChineseBlackjackReport(filter);

        public Task<IEnumerable<SlotGameBetRecord>> GetSlotGameReport(GetSlotGameReportRequest request)
            => reportApi.GetSlotGameReport(
                request.RoundId,
                request.Nickname,
                request.ExcludeUUS,
                request.Game,
                request.PartnerId,
                request.FromDate,
                request.ToDate,
                request.Username,
                request.ConvertToUSD);

        public Task<IEnumerable<JackpotRecord>> GetJackpotReport(int gameId, DateTime fromDate, DateTime toDate, bool excludeUUS, bool excludeBot, string operatorId, IEnumerable<string> siteNames)
            => reportApi.GetJackpotReport(gameId, fromDate, toDate, excludeUUS, excludeBot, operatorId, siteNames);

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

        public Task<IEnumerable<CascadingSlotBetRecord>> GetCascadingSlotReport(CascadingSlotReportFilter filter)
            => reportApi.GetCascadingSlotReport(filter);

        public Task<CascadingSlotBetHistory> GetCascadingSlotBetHistory(CascadingSlotReportFilter filter)
            => reportApi.GetCascadingSlotBetHistory(filter);

        public Task<CrashGameBetReport> GetCrashGameReport(CrashGameBetReportFilter filter)
           => reportApi.GetCrashGameReport(filter);

        public Task<IEnumerable<CrashTurnGameBetRecord>> GetCrashTurnGameReport(CrashTurnGameBetReportFilter filter)
           => reportApi.GetCrashTurnGameReport(filter);

        public Task<IEnumerable<JackpotPool>> GetJackpotRunningPools(int gameId, string operatorId, IEnumerable<string> siteNames)
            => reportApi.GetJackpotRunningPools(gameId, operatorId, siteNames);
    }
}