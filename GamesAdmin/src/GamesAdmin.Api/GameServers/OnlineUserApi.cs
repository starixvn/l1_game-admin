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

    public interface IVipSedieWarOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IFourGuardiansOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoProMaxOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IMiniSeDieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IRouletteOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IBaccaratOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface ILotteryOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IFishPrawnCrabOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IDragonTigerOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IBigSmallOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface ITurboBigSmallOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface ITurboOddEvenOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IThreeCardsOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoMaxOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoMax2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoMiniOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoMini2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoEastOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoWestOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoSouthOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IKenoNorthOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }
}
