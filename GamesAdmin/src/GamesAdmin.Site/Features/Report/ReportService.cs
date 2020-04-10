using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Report;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IReportService
    {
        Task<IEnumerable<BigSmallRecord>> GetBigSmallReport(long roundId, string nickname, bool excludeBot);

        Task<IEnumerable<Round>> GetBigSmallRounds();
    }

    public class ReportService : IReportService
    {
        private readonly IReportApi reportApi;

        public ReportService(IReportApi reportApi)
        {
            this.reportApi = reportApi;
        }

        public async Task<IEnumerable<BigSmallRecord>> GetBigSmallReport(long roundId, string nickname, bool excludeBot)
        => await reportApi.GetBigSmallReport(roundId, nickname, excludeBot);

        public async Task<IEnumerable<Round>> GetBigSmallRounds() => await reportApi.GetBigSmallRounds();
    }
}