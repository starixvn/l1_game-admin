using GamesAdmin.Core.Models.BetReport;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Report
{
    [Headers("Authorization: Bearer")]
    public interface IReportApi
    {
        [Get("/api/report/bethistory")]
        Task<BetHistory> GetBetHistory(BetHistoryFilter filter);
    }
}