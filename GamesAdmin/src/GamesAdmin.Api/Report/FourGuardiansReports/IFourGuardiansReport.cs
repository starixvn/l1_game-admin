using GamesAdmin.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface IFourGuardiansReport
    {
        Task<IEnumerable<FourGuardiansBetRecord>> GetReportAsync(long handId, string nickname, bool excludeUUS);
    }
}
