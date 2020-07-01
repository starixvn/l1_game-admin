using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Api.Report
{
    public interface IKenoReport
    {
        Task<KenoBetReport> GetReportAsync(long roundId);

        Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname);

        Task<KenoRoundResult> GetRoundResultAsync(long roundId);
    }
}
