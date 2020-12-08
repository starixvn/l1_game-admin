using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IOnlineUserApi
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

    public interface ISpecialSedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IBlackjackOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IVipFishPrawnCrabOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IVipBaccaratOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface ISicBoOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }
}
