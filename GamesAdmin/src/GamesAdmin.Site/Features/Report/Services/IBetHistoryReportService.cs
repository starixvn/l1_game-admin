using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;

namespace GamesAdmin.Site.Features.Report
{
    public interface IBetHistoryReportService
    {
        Task<BetHistoryViewModel> Get(BetHistoryFilter filter);
    }
}