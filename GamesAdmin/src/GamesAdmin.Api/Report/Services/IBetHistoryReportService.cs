using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Api.Report
{
    public interface IBetHistoryReportService
    {
        Task<BetHistory> Get(BetHistoryFilter filter);
    }
}