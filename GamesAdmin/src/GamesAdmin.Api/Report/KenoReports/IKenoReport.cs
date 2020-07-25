using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;

namespace GamesAdmin.Api.Report
{
    public interface IKenoReport
    {
        Task<IEnumerable<KenoBetRecord>> GetReportAsync(long roundId, string nickname, bool excludeUUS);

        Task<IEnumerable<KenoBetHistory>> GetBetHistoryAsync(long roundId, string nickname);

        Task<KenoRoundResult> GetRoundResultAsync(long roundId);
    }
}
