using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    public interface IReportApi
    {
        [Get("/api/report")]
        Task<T> GetBetReport<T>(BetReportFilter filter, [Header("Authorization")] string header = "")
            where T : BetReportBase;

        [Get("/api/report/bethistory")]
        Task<T> GetBetHistory<T>(BetHistoryFilter filter, [Header("Authorization")] string header = "")
            where T : BetHistory;
    }
}