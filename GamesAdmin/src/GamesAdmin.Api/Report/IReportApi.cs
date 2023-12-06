using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    [Headers("Authorization: Bearer")]
    public interface IReportApi
    {
        [Get("/api/report")]
        Task<T> GetBetReport<T>(BetReportFilter filter)
            where T : BetReportBase;

        [Get("/api/report/bethistory")]
        Task<BetHistory> GetBetHistory(BetHistoryFilter filter);
    }
}