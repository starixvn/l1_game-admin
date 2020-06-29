using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report
{
    public interface IKenoReport
    {
        Task<KenoBetReport> GetReportAsync(long roundId);

        Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname);
    }
}
