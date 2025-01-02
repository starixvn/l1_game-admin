using System;
using System.Collections.Generic;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface IOnlineUserApiFactory
    {
        IOnlineUserApi GetOnlineUserApi(GameConfig gameConfig);
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
            
            { Game.KenoMaxValue, typeof(IKenoMaxOnlineUserApi) },
            { Game.KenoMax2Value, typeof(IKenoMax2OnlineUserApi) },
            { Game.BitcoinMasterValue, typeof(IBitcoinMasterOnlineUserApi) },
            { Game.CasinoHoldemPokerValue, typeof(ICasinoHoldemPokerOnlineUserApi) },
            { Game.HiLoValue, typeof(IHiLoApi) },
            { Game.TexasHoldemPokerValue, typeof(ITexasHoldemPokerOnlineUserApi) },
            { Game.TreasureIslandValue, typeof(ITreasureIslandOnlineUserApi) },
            { Game.BlockchainBaccaratValue, typeof(IBlockchainBaccaratOnlineUserApi) },
            { Game.MiniSeDie2Value, typeof(IMiniSeDieOnlineUserApi) },
            { Game.MiniSeDie3Value, typeof(IMiniSeDieOnlineUserApi) },
            { Game.MiniSeDie4Value, typeof(IMiniSeDieOnlineUserApi) },
            { Game.UnbalancedBigSmall1Value, typeof(IUnbalancedBigSmallOnlineUserApi) },
            { Game.UnbalancedBigSmall2Value, typeof(IUnbalancedBigSmallOnlineUserApi) },
            { Game.UnbalancedBigSmall3Value, typeof(IUnbalancedBigSmallOnlineUserApi) },
            { Game.UnbalancedBigSmall4Value, typeof(IUnbalancedBigSmallOnlineUserApi) },
            { Game.FruitSlotValue, typeof(IFruitSlotOnlineUserApi) },
            { Game.FanTanValue, typeof(IFanTanOnlineUserApi) },
            { Game.WinThreeCardsValue, typeof(IWinThreeCardsOnlineUserApi) },
            { Game.DragonTigerFightValue, typeof(IDragonTigerFightOnlineUserApi) },
            { Game.PontoonValue, typeof(IPontoonOnlineUserApi) },
            { Game.ColorGameValue, typeof(IColorGameOnlineUserApi) },
            { Game.BandarQiuQiuValue, typeof(IBandarQiuQiuOnlineUserApi) },
            { Game.ChineseBlackjackValue, typeof(IChineseBlackjackOnlineUserApi) },
            { Game.BacBall1Value, typeof(IBacBall1OnlineUserApi) },
            { Game.BacBall2Value, typeof(IBacBall2OnlineUserApi) },
            { Game.LuckyNext1Value, typeof(ILuckyNext1OnlineUserApi) },
            { Game.LuckyNext2Value, typeof(ILuckyNext2OnlineUserApi) },
            { Game.FootballDiceValue, typeof(IFootballDiceOnlineUserApi) },
            { Game.FootballSlotValue, typeof(IFootballSlotOnlineUserApi) },
            { Game.CoinTossValue, typeof(ICoinTossOnlineUserApi) },
            { Game.SeBall1Value, typeof(ISeBall1OnlineUserApi) },
            { Game.LuckyThreeValue, typeof(ILuckyThreeOnlineUserApi) },
            { Game.SicBallValue, typeof(ISicBallOnlineUserApi) },
            { Game.MegaKenoValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.MegaKeno2Value, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoMiniValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoMini2Value, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoEastValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoWestValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoSouthValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoNorthValue, typeof(ISKMiniKenoOnlineUserApi) },
            { Game.KenoPowerValue, typeof(IKenoPowerOnlineUserApi) },
            { Game.KenoPower2Value, typeof(IKenoPowerOnlineUserApi) },
            { Game.KenoSoccerValue, typeof(IKenoMatchOnlineUserApi) },
            { Game.KenoSoccer2Value, typeof(IKenoMatchOnlineUserApi) },
        };

        private readonly IServiceProvider serviceProvider;

        private readonly IAppSettings appSettings;

        public OnlineUserApiFactory(IServiceProvider serviceProvider, IAppSettings appSettings)
        {
            this.serviceProvider = serviceProvider;
            this.appSettings = appSettings;
        }

        IOnlineUserApi IOnlineUserApiFactory.GetOnlineUserApi(GameConfig gameConfig)
        {
            var gameConfigName = gameConfig.Info.Name;
            if (onlineUserServices.ContainsKey(gameConfigName))
            {
                return (IOnlineUserApi)serviceProvider.GetService(onlineUserServices[gameConfigName]);
            }
            else
            {
                var refitSettings = new RefitSettings
                {
                    AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
                };
                var displayName = gameConfig.Info.DisplayName;
                if (string.IsNullOrEmpty(displayName)) 
                {
                    displayName = Game.FromGameName(gameConfig.Info.Name).DisplayName;
                }
                var serviceName = NamingConverter.ServiceName(displayName, appSettings.ServiceNameSeparator);
                var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

                return RestService.For<ISlotGameOnlineUserApi>(api, refitSettings);
            }
        }
    }
}