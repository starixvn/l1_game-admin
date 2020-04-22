using System.Threading.Tasks;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IStatisticApi : IBaseAuthorizationApi
    {
        [Get("/statistic/TodayTotalBets")]
        Task<long> GetTodayTotalBets();
    }
}