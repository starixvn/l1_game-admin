using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Report
{
    public interface IReportApi : IBaseAuthorizationApi
    {

        [Get("/report/big_small/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<BigSmallReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot);
    }
}