using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface ISlotGameReport
    {
        Task<IEnumerable<SlotGameBetRecord>> GetReportAsync(long handId, string nickname, bool excludeUUS);
    }
}
