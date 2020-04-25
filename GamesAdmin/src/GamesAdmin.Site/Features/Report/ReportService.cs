using System.Collections.Generic;
using System.Threading.Tasks;
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

        Task<IEnumerable<BaseBetHistory>> GetBetHistory(byte gameTypeId, long roundId, string nickname);
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

        public async Task<IEnumerable<BaseBetHistory>> GetBetHistory(byte gameTypeId, long roundId, string nickname) 
        => await reportApi.GetBetHistory(gameTypeId, roundId, nickname);
    }
}