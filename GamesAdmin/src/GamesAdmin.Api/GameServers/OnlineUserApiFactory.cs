using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Api.GameServers
{
    public interface IOnlineUserApiFactory
    {
        IOnlineUserApi GetOnlineUserApi(Game game);
    }

    public class OnlineUserApiFactory : IOnlineUserApiFactory
    {
        private readonly Dictionary<string, Type> onlineUserServices = new Dictionary<string, Type>
        {
            { Game.SeDieValue, typeof(ISedieOnlineUserApi) },
            { Game.VipSeDieValue, typeof(IVipSedieOnlineUserApi) },
            { Game.SpecialSeDieValue, typeof(ISpecialSedieOnlineUserApi) },
            { Game.BlackJackValue, typeof(IBlackjackOnlineUserApi) },
            { Game.VipFishPrawnCrabValue, typeof(IVipFishPrawnCrabOnlineUserApi) },
            { Game.SicBoValue, typeof(ISicBoOnlineUserApi) },
            { Game.VipBaccaratValue, typeof(IVipBaccaratOnlineUserApi) },
            { Game.VipSeDieWarValue, typeof(IVipSedieWarOnlineUserApi) },
            { Game.FourGuardiansValue, typeof(IFourGuardiansOnlineUserApi) },
            { Game.KenoProMaxValue, typeof(IKenoProMaxOnlineUserApi) },
            { Game.MiniSeDieValue, typeof(IMiniSeDieOnlineUserApi) },
            { Game.RouletteValue, typeof(IRouletteOnlineUserApi) },
            { Game.BaccaratValue, typeof(IBaccaratOnlineUserApi) },
            { Game.LotteryValue, typeof(ILotteryOnlineUserApi) },
            { Game.FishPrawnCrabValue, typeof(IFishPrawnCrabOnlineUserApi) },
            { Game.DragonTigerValue, typeof(IDragonTigerOnlineUserApi) },
            { Game.BigSmallValue, typeof(IBigSmallOnlineUserApi) },
            { Game.BigSmallTurboValue, typeof(ITurboBigSmallOnlineUserApi) },
            { Game.OddEvenTurboValue, typeof(ITurboOddEvenOnlineUserApi) },
            { Game.ThreeCardsValue, typeof(IThreeCardsOnlineUserApi) },
            { Game.KenoMatch1Value, typeof(IKenoMatchOnlineUserApi) },
            { Game.KenoMatch2Value, typeof(IKenoMatchOnlineUserApi) },
            { Game.KenoPower1Value, typeof(IKenoPowerOnlineUserApi) },
            { Game.KenoPower2Value, typeof(IKenoPowerOnlineUserApi) },
            { Game.HeroesSlotValue, typeof(IHeroesSlotOnlineUserApi) },
            { Game.KenoMaxValue, typeof(IKenoMaxOnlineUserApi) },
            { Game.KenoMax2Value, typeof(IKenoMax2OnlineUserApi) },
            { Game.KenoMiniValue, typeof(IKenoMiniOnlineUserApi) },
            { Game.KenoMini2Value, typeof(IKenoMini2OnlineUserApi) },
            { Game.KenoEastValue, typeof(IKenoEastOnlineUserApi) },
            { Game.KenoWestValue, typeof(IKenoWestOnlineUserApi) },
            { Game.KenoSouthValue, typeof(IKenoSouthOnlineUserApi) },
            { Game.KenoNorthValue, typeof(IKenoNorthOnlineUserApi) },
            { Game.BitcoinMasterValue, typeof(IBitcoinMasterOnlineUserApi) },
            { Game.CasinoHoldemPokerValue, typeof(ICasinoHoldemPokerOnlineUserApi) },
            { Game.HiLoValue, typeof(IHiLoApi) },
            { Game.TexasHoldemPokerValue, typeof(ITexasHoldemPokerOnlineUserApi) },
            { Game.NightClubValue, typeof(INightClubOnlineUserApi) },
            { Game.TreasureIslandValue, typeof(ITreasureIslandOnlineUserApi) },
            { Game.BlockchainBaccaratValue, typeof(IBlockchainBaccaratOnlineUserApi) },
            { Game.MegaKenoValue, typeof(IMegaKenoOnlineUserApi) },
            { Game.MegaKeno2Value, typeof(IMegaKeno2OnlineUserApi) },
        };

        private readonly Type defaultOnlineUserApi = typeof(IMainOnlineUserApi);

        private readonly IServiceProvider serviceProvider;

        public OnlineUserApiFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        IOnlineUserApi IOnlineUserApiFactory.GetOnlineUserApi(Game game)
        => onlineUserServices.ContainsKey(game.Value)
            ? (IOnlineUserApi)serviceProvider.GetService(onlineUserServices[game.Value])
            : (IOnlineUserApi)serviceProvider.GetService(defaultOnlineUserApi);
    }
}