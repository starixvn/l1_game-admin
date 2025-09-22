using GamesAdmin.Core.Models.BetReport;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Report
{
    public interface IRemoteReportApi
    {
        [Post("/RoundHistory/GetAccessUrl")]
        Task<string> GetAccessUrl([Body] BetReportFilter model);
    }
}
