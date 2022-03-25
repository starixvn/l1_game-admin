using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IUserServiceApi
    {
        [Get("/admin/online-users")]
        Task<OnlineUsers> GetAllOnlineUsers();
    }
}
