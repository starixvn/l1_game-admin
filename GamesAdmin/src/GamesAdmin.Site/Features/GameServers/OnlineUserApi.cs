using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.GameServers
{
    public interface IOnlineUserApi : IBaseAuthorizationApi
    {
        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IMainOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/game/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name);
    }

    public interface ISedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IVipSedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }
}
