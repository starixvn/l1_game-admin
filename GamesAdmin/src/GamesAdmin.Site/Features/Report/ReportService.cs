using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Report;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IReportService
    {
        Task<GameReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot);

        Task<GameReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot);

        Task<GameReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot);

        Task<GameReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot);

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

        public async Task<GameReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetBigSmallReport(roundId, nickname, excludeBot);

        public async Task<GameReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetBigSmallTurboReport(roundId, nickname, excludeBot);

        public async Task<GameReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetOddEvenReport(roundId, nickname, excludeBot);

        public async Task<GameReport> GetOddEventTurboReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetOddEvenTurboReport(roundId, nickname, excludeBot);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameType.BigSmall, roundId, nickname);

        public async Task<IEnumerable<BigSmallBet>> GetBigSmallTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<BigSmallBet>((byte)GameType.BigSmallTurbo, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameType.EvenOdd, roundId, nickname);

        public async Task<IEnumerable<OddEvenBet>> GetOddEvenTurboBetHistory(long roundId, string nickname)
        => await reportApi.GetBetHistory<OddEvenBet>((byte)GameType.EvenOddTurbo, roundId, nickname);
    }
}