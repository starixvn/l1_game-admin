using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IReportService
    {
        Task<BigSmallBetReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot);

        Task<BigSmallBetReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot);

        Task<OddEvenBetReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot);

        Task<SeDieBetReport> GetSeDieReport(long roundId, string nickname, bool excludeBot);

        Task<SeDieBetReport> GetMiniSeDieReport(long roundId, string nickname, bool excludeBot);
        
        Task<KenoBetReport> GetKenoProVietBetReport(long roundId, string nickname, bool excludeBot);

        Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname);

        Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname);

        Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname);
    }

    public class ReportService : IReportService
    {
        private readonly IReportApi reportApi;

        public ReportService(IReportApi reportApi)
        {
            this.reportApi = reportApi;
        }

        public async Task<BigSmallBetReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetBigSmallReport(roundId, nickname, excludeBot);

        public async Task<BigSmallBetReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetBigSmallTurboReport(roundId, nickname, excludeBot);

        public async Task<OddEvenBetReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetOddEvenReport(roundId, nickname, excludeBot);

        public async Task<OddEvenBetReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetOddEvenTurboReport(roundId, nickname, excludeBot);

        public async Task<SeDieBetReport> GetSeDieReport(long roundId, string nickname, bool excludeBot)
            => await reportApi.GetSeDieReport(roundId, nickname, excludeBot);

        public async Task<SeDieBetReport> GetMiniSeDieReport(long roundId, string nickname, bool excludeBot)
            => await reportApi.GetMiniSeDieReport(roundId, nickname, excludeBot);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmall, roundId, nickname);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameId.BigSmallTurbo, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.EvenOdd, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameId.EvenOddTurbo, roundId, nickname);

        public Task<KenoBetReport> GetKenoProVietBetReport(long roundId, string nickname, bool excludeBot)
        => reportApi.GetKenoProVietReport(GameType.KenoProVietValue, roundId, nickname, excludeBot);
    }
}