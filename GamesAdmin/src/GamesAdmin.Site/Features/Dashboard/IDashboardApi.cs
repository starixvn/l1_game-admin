using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IStatisticGameServerApi : IBaseAuthorizationApi
    {
        [Get("/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();
    }

    public interface IStatisticApi : IBaseAuthorizationApi
    {
        [Get("/statistic/TodayTotalBets")]
        Task<long> GetTodayTotalBets();
    }
}