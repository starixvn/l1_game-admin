using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report
{
    public interface IGameReportService
    {
        Task<BetReportBase> Get(BetReportFilter filter);
    }
}