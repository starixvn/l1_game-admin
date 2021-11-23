using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report
{
    public interface ISlotGameReport
    {
        Task<IEnumerable<SlotGameBetRecord>> GetReportAsync(SlotGameBetReportFilter filter);
    }
}
