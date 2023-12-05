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
        Task<BetReport<TRoundResult, TBetRecord>> GetBetReport<TRoundResult, TBetRecord>(BetReportFilter filter)
            where TRoundResult : RoundResult where TBetRecord : BetRecord;

        [Get("/api/report/bethistory")]
        Task<BetHistory> GetBetHistory(BetHistoryFilter filter);
    }
}