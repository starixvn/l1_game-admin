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
            { Game.KenoMatch1Value, typeof(IKenoMatchOnlineUserApi) },
            { Game.KenoMatch2Value, typeof(IKenoMatchOnlineUserApi) },
            { Game.KenoPower1Value, typeof(IKenoPowerOnlineUserApi) },
            { Game.KenoPower2Value, typeof(IKenoPowerOnlineUserApi) },
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
            { Game.TreasureIslandValue, typeof(ITreasureIslandOnlineUserApi) },
            { Game.BlockchainBaccaratValue, typeof(IBlockchainBaccaratOnlineUserApi) },
            { Game.MegaKenoValue, typeof(IMegaKenoOnlineUserApi) },
            { Game.MegaKeno2Value, typeof(IMegaKeno2OnlineUserApi) },
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
            { Game.Bac81Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.SeDie3Value, typeof(ISeDie3OnlineUserApi) },
            { Game.KaraKrusValue, typeof(IKaraKrusOnlineUserApi) },
            { Game.FootballCoinTossValue, typeof(IFootballCoinTossOnlineUserApi) },
            { Game.Bac82Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.MarbleFootballValue, typeof(IMarbleFootballOnlineUserApi) },
            { Game.Bac83Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac84Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac85Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac86Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac87Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac88Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac89Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac90Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac91Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac92Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac97Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac98Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac100Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.Bac101Value, typeof(IBaccaratXOnlineUserApi) },
            { Game.SevenUpDownValue, typeof(ISevenUpDownOnlineUserApi) },
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
                    ContentSerializer = new NewtonsoftJsonContentSerializer(),
                    AuthorizationHeaderValueGetter = async (request, cancellationToken) => await IdentityServerManager.GetAccessToken()
                };
                var serviceName = NamingConverter.ServiceName(gameConfig.Info.DisplayName, appSettings.ServiceNameSeparator);
                var api = string.Format(appSettings.ServiceApiTemplate, serviceName);

                return RestService.For<ISlotGameOnlineUserApi>(api, refitSettings);
            }
        }
    }
}