using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report
{
    public interface ICascadingSlotReportService
    {
        Task<IEnumerable<CascadingSlotBetRecord>> GetReportAsync(CascadingSlotReportFilter filter);

        Task<CascadingSlotBetHistory> GetBetHistoryAsync(CascadingSlotReportFilter filter);
    }
}
