using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.Dashboard
{
    public interface IDashboardGameServerApi : IBaseAuthorizationApi
    {
        [Get("/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();
    }

    public interface IDashboardApi : IBaseAuthorizationApi
    {
        [Get("/dashboard/TodayTotalBets")]
        Task<long> GetTodayTotalBets();
    }
}