using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using Refit;
using HiloCard = GamesAdmin.Core.Models.Card;

namespace GamesAdmin.Api.GameServers
{
    public interface IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IMainOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/game/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name, [Header("Authorization")] string header = "");
    }

    public interface ISedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IVipSedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ISpecialSedieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBlackjackOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IVipFishPrawnCrabOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IVipBaccaratOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ISicBoOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IVipSedieWarOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoProMaxOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IMiniSeDieOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IRouletteOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBaccaratOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ILotteryOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IFishPrawnCrabOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IDragonTigerOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBigSmallOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ITurboBigSmallOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ITurboOddEvenOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IThreeCardsOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoMaxOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenomax/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoMax2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenomax2/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoMiniOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenomini/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoMini2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenomini2/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoEastOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenoeast/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoWestOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenowest/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoSouthOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenosouth/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoNorthOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/kenonorth/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBitcoinMasterOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoMatchOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IKenoPowerOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ITexasHoldemPokerOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ICasinoHoldemPokerOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IHiLoApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");

        [Post("/api/set-result")]
        Task<IEnumerable<HiloCard>> SetResult([Body] SetHiLoResultRequest request, [Header("Authorization")] string header = "");
    }

    public interface ITreasureIslandOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBlockchainBaccaratOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IUnbalancedBigSmallOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IMegaKenoOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/megakeno/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IMegaKeno2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/megakeno2/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IFruitSlotOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IFanTanOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IWinThreeCardsOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IDragonTigerFightOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ISlotGameOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IPontoonOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IColorGameOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBandarQiuQiuOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBacBall1OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IBacBall2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IChineseBlackjackOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface ILuckyNext1OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ILuckyNext2OnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface IFootballDiceOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IFootballSlotOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }

    public interface ICoinTossOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "");
    }

    public interface IBaccaratXOnlineUserApi : IOnlineUserApi
    {
        [Get("/api/{name}/onlineusers")]
        new Task<OnlineUsers> GetOnlineUsers(string name = "", [Header("Authorization")] string header = "");
    }
}