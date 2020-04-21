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
        Task<GameReport> GetBigSmallReport(long roundId, string nickname, bool excludeBot);


        [Get("/report/big_small_turbo/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<GameReport> GetBigSmallTurboReport(long roundId, string nickname, bool excludeBot);


        [Get("/report/odd_even/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<GameReport> GetOddEvenReport(long roundId, string nickname, bool excludeBot);


        [Get("/report/odd_even_turbo/round/{roundId}/users/{nickname}?excludeBot={excludeBot}")]
        Task<GameReport> GetOddEvenTurboReport(long roundId, string nickname, bool excludeBot);
    }
}