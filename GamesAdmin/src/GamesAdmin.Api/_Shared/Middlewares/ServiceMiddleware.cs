using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.ApiLogs;
using GamesAdmin.Api.ApiLogs.Apis;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Chips;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroupGames;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.GameGroupManagement;
using GamesAdmin.Api.Features.OperateSettings;
using GamesAdmin.Api.GameManagementApi;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameServers;
using GamesAdmin.Api.GameSettings;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.GeneralGameSettings;
using GamesAdmin.Api.GlobalConfigs;
using GamesAdmin.Api.Lobby;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.PartnerRemote;
using GamesAdmin.Api.Partners;
using GamesAdmin.Api.PlayerInfo;
using GamesAdmin.Api.Promotions;
using GamesAdmin.Api.Public;
using GamesAdmin.Api.Report;
using GamesAdmin.Api.Report.Services;
using GamesAdmin.Api.ServiceStatus;
using GamesAdmin.Api.UM;
using GamesAdmin.Api.Users;
using GamesAdmin.Api.VoidGame;
using GamesAdmin.Api.VoidGameRemote;
using GamesAdmin.Core;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Refit;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class ServiceMiddleware
    {
        public static void AddServices(this IServiceCollection services, IAppSettings appSettings)
        {
            IdentityServerManager.Initialize(appSettings.IdentityServerAuth);

            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken,
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                })
            };

            services.AddScoped<IHiLoApiFactory, HiLoApiFactory>();
            services.AddScoped<IGameSettingsService, GameSettingsService>();
            services.AddScoped<ICloneSettingService, CloneSettingsService>();
            services.AddScoped<IGeneralGameSettingService, GeneralGameSettingService>();
            services.AddScoped<ICurrencyGroupGameService, CurrencyGroupGameService>();
            services.AddScoped<IUMService, UMService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientSiteSiteSerivce, ClientSiteSerivce>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IGameUserService, GameUserService>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IGlobalConfigService, GlobalConfigService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyGroupService, CurrencyGroupService>();
            services.AddScoped<IChipService, ChipService>();
            services.AddScoped<IHoldemPokerReportService, HoldemPokerReportService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromotionCampaignService, PromotionCampaignService>();
            services.AddScoped<IAwardService, AwardService>();
            services.AddScoped<ILobbyService, LobbyService>();
            services.AddScoped<IDomainService, DomainService>();
            services.AddScoped<IRTPVerifyService, RTPVeirfyService>();
            services.AddScoped<IRoundWatcherService, RoundWatcherService>();
            services.AddScoped<IGameServerService, GameServerService>();
            services.AddScoped<IStatusCheckerService, StatusCheckerService>();
            services.AddScoped<IGameGroupService, GameGroupService>();
            services.AddScoped<IInternalTestAccountService, InternalTestAccountService>();
            services.AddScoped<IPlayerInfoService, PlayerInfoService>();

            services.AddSingleton(RestService.For<IGameServerApi>(appSettings.GameServers.Main.Api, refitSettings));
            services.AddSingleton(RestService.For<IUserServiceApi>(appSettings.GameServers.UserService.Api, refitSettings));
            services.AddSingleton(RestService.For<INotificationServiceApi>(appSettings.GameServers.NotificationService.Api, refitSettings));
            services.AddSingleton(RestService.For<IMainOnlineUserApi>(appSettings.GameServers.UserService.Api, refitSettings));
            services.AddSingleton<ICacheManager, CacheManager>();

            if (appSettings.SystemType == SystemType.GN)
            {
                AddOnlineUserApiGN(services, appSettings, refitSettings);
            }
            else if (appSettings.SystemType == SystemType.SC1 || appSettings.SystemType == SystemType.VG2 || appSettings.SystemType == SystemType.HD)
            {
                AddOnlineUserApiSCVG(services, appSettings, refitSettings);
            }

            services.AddScoped<IOnlineUserApiFactory, OnlineUserApiFactory>();
            services.AddSingleton(RestService.For<IRoundWatcherServiceApi>(appSettings.RoundWatcherServiceSettings.Url, refitSettings));
            services.AddSingleton(RestService.For<IStatusServiceApi>(appSettings.StatusCheckerServiceSettings.Url, refitSettings));
            services.AddSingleton(RestService.For<IRTPServiceApi>(appSettings.RtpCheckerServiceSettings.Url, refitSettings));

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.SabaApiSettings.Url, refitSettings));
            services.AddSingleton(RestService.For<IAwardGameApi>(appSettings.SabaApiSettings.Url, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();
            services.AddScoped<ITicketHandler, TicketHandler>();

            AddSlotServices(services);
            AddHoldemPokerServices(services, appSettings, refitSettings);
            AddChatBotServices(services, appSettings);
            AddVoidRunningTicketServices(services);
            AddPontoonServices(services, appSettings, refitSettings);
            AddCrashGameServices(services);
            AddFootballDiceServices(services, appSettings, refitSettings);
            AddChineseBlackjackServices(services, appSettings, refitSettings);
            AddBaccaratXServices(services, appSettings, refitSettings);

            services.AddScoped<IReportServiceFactory, ReportServiceFactory>();
            services.AddScoped<SeDieReportService>();
            services.AddScoped<MiniSeDieReportService>();
            services.AddScoped<FishPrawnCrabReportService>();
            services.AddScoped<DragonTigerReportService>();
            services.AddScoped<VipSeDieReportService>();
            services.AddScoped<BlackjackReportService>();
            services.AddScoped<PontoonReportService>();
            services.AddScoped<SicBoReportService>();
            services.AddScoped<VipFishPrawnCrabReportService>();
            services.AddScoped<LotteryReportService>();
            services.AddScoped<ThreeCardsReportService>();
            services.AddScoped(typeof(KenoMatchReportService<,,>));
            services.AddScoped(typeof(KenoPowerReportService<,,>));
            services.AddScoped(typeof(MiniKenoReportService<,,>));
            services.AddScoped<VipSeDieWarReportService>();
            services.AddScoped<KenoProMaxReportService>();
            services.AddScoped<HiLoReportService>();
            services.AddScoped<FruitSlotReportService>();
            services.AddScoped<WinThreeCardsReportService>();

            services.AddScoped<IReportServiceFactory, ReportServiceFactory>();
            services.AddScoped<MiniSeDieReportService>();
            services.AddScoped<FishPrawnCrabReportService>();
            services.AddScoped<DragonTigerReportService>();
            services.AddScoped<RouletteReportService>();
            services.AddScoped<VipSeDieReportService>();
            services.AddScoped<BlackjackReportService>();
            services.AddScoped<SicBoReportService>();
            services.AddScoped(typeof(UnbalancedBigSmallReportService<,,>));

            services.AddScoped<BaccaratReportService>();
            services.AddScoped(typeof(MiniSeDieMultiplayReportService<,>));
            services.AddScoped<FanTanReportService>();
            services.AddScoped<DragonTigerFightReportService>();
            services.AddScoped<ColorGameReportService>();
            services.AddScoped<BandarQiuQiuReportService>();
            services.AddScoped<ChineseBlackjackReportService>();
            services.AddScoped<BacBall1ReportService>();
            services.AddScoped<BacBall2ReportService>();
            services.AddScoped(typeof(NextBallReportService<,,>));
            services.AddScoped<FootballDiceReportService>();
            services.AddScoped<FootballSlotReportService>();

            services.AddPublicService();

            services.InitApiLog();

            services.AddCloudServices(appSettings);

            OperatorSettingsServiceRegister.Register(services);
        }

        private static void AddOnlineUserApiSCVG(IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            if (appSettings.GameServers.Sedie?.Api != null)
                services.AddSingleton(RestService.For<ISedieOnlineUserApi>(appSettings.GameServers.Sedie.Api, refitSettings));
            
            if (appSettings.GameServers.VipSedie?.Api != null)
                services.AddSingleton(RestService.For<IVipSedieOnlineUserApi>(appSettings.GameServers.VipSedie.Api, refitSettings));
            
            if (appSettings.GameServers.SpecialSedie?.Api != null)
                services.AddSingleton(RestService.For<ISpecialSedieOnlineUserApi>(appSettings.GameServers.SpecialSedie.Api, refitSettings));
            
            if (appSettings.GameServers.Blackjack?.Api != null)
                services.AddSingleton(RestService.For<IBlackjackOnlineUserApi>(appSettings.GameServers.Blackjack.Api, refitSettings));

            if (appSettings.GameServers.VipFishPrawnCrab?.Api != null)
                services.AddSingleton(RestService.For<IVipFishPrawnCrabOnlineUserApi>(appSettings.GameServers.VipFishPrawnCrab.Api, refitSettings));
            
            if (appSettings.GameServers.VipBaccarat?.Api != null)
                services.AddSingleton(RestService.For<IVipBaccaratOnlineUserApi>(appSettings.GameServers.VipBaccarat.Api, refitSettings));

            if (appSettings.GameServers.SicBo?.Api != null)
                services.AddSingleton(RestService.For<ISicBoOnlineUserApi>(appSettings.GameServers.SicBo.Api, refitSettings));
            
            if (appSettings.GameServers.VipSedieWar?.Api != null)
                services.AddSingleton(RestService.For<IVipSedieWarOnlineUserApi>(appSettings.GameServers.VipSedieWar.Api, refitSettings));
            
            if (appSettings.GameServers.KenoProMax?.Api != null)
                services.AddSingleton(RestService.For<IKenoProMaxOnlineUserApi>(appSettings.GameServers.KenoProMax.Api, refitSettings));
            
            if (appSettings.GameServers.MiniSeDie?.Api != null)
                services.AddSingleton(RestService.For<IMiniSeDieOnlineUserApi>(appSettings.GameServers.MiniSeDie.Api, refitSettings));
            
            if (appSettings.GameServers.Roulette?.Api != null)
                services.AddSingleton(RestService.For<IRouletteOnlineUserApi>(appSettings.GameServers.Roulette.Api, refitSettings));
            
            if (appSettings.GameServers.Baccarat?.Api != null)
                services.AddSingleton(RestService.For<IBaccaratOnlineUserApi>(appSettings.GameServers.Baccarat.Api, refitSettings));
            
            if (appSettings.GameServers.FishPrawnCrab?.Api != null)
                services.AddSingleton(RestService.For<IFishPrawnCrabOnlineUserApi>(appSettings.GameServers.FishPrawnCrab.Api, refitSettings));
            
            if (appSettings.GameServers.DragonTiger?.Api != null)
                services.AddSingleton(RestService.For<IDragonTigerOnlineUserApi>(appSettings.GameServers.DragonTiger.Api, refitSettings));
            
            if (appSettings.GameServers.BigSmall?.Api != null)
                services.AddSingleton(RestService.For<IBigSmallOnlineUserApi>(appSettings.GameServers.BigSmall.Api, refitSettings));
            
            if (appSettings.GameServers.TurboBigSmall?.Api != null)
                services.AddSingleton(RestService.For<ITurboBigSmallOnlineUserApi>(appSettings.GameServers.TurboBigSmall.Api, refitSettings));
            
            if (appSettings.GameServers.TurboOddEven?.Api != null)
                services.AddSingleton(RestService.For<ITurboOddEvenOnlineUserApi>(appSettings.GameServers.TurboOddEven.Api, refitSettings));
            
            if (appSettings.GameServers.Lottery?.Api != null)
                services.AddSingleton(RestService.For<ILotteryOnlineUserApi>(appSettings.GameServers.Lottery.Api, refitSettings));
            
            if (appSettings.GameServers.ThreeCards?.Api != null)
                services.AddSingleton(RestService.For<IThreeCardsOnlineUserApi>(appSettings.GameServers.ThreeCards.Api, refitSettings));
            
            if (appSettings.GameServers.KenoMatch1?.Api != null)
                services.AddSingleton(RestService.For<IKenoMatchOnlineUserApi>(appSettings.GameServers.KenoMatch1.Api, refitSettings));
            
            if (appSettings.GameServers.KenoPower1?.Api != null)
                services.AddSingleton(RestService.For<IKenoPowerOnlineUserApi>(appSettings.GameServers.KenoPower1.Api, refitSettings));
            
            if (appSettings.GameServers.KenoMax?.Api != null)
                services.AddSingleton(RestService.For<IKenoMaxOnlineUserApi>(appSettings.GameServers.KenoMax.Api, refitSettings));
            
            if (appSettings.GameServers.KenoMax2?.Api != null)
                services.AddSingleton(RestService.For<IKenoMax2OnlineUserApi>(appSettings.GameServers.KenoMax2.Api, refitSettings));
            
            if (appSettings.GameServers.KenoMini?.Api != null)
                services.AddSingleton(RestService.For<IKenoMiniOnlineUserApi>(appSettings.GameServers.KenoMini.Api, refitSettings));
            
            if (appSettings.GameServers.KenoMini2?.Api != null)
                services.AddSingleton(RestService.For<IKenoMini2OnlineUserApi>(appSettings.GameServers.KenoMini2.Api, refitSettings));
            
            if (appSettings.GameServers.KenoEast?.Api != null)
                services.AddSingleton(RestService.For<IKenoEastOnlineUserApi>(appSettings.GameServers.KenoEast.Api, refitSettings));
            
            if (appSettings.GameServers.KenoWest?.Api != null)
                services.AddSingleton(RestService.For<IKenoWestOnlineUserApi>(appSettings.GameServers.KenoWest.Api, refitSettings));
            
            if (appSettings.GameServers.KenoSouth?.Api != null)
                services.AddSingleton(RestService.For<IKenoSouthOnlineUserApi>(appSettings.GameServers.KenoSouth.Api, refitSettings));
            
            if (appSettings.GameServers.KenoNorth?.Api != null)
                services.AddSingleton(RestService.For<IKenoNorthOnlineUserApi>(appSettings.GameServers.KenoNorth.Api, refitSettings));
            
            if (appSettings.GameServers.BitcoinMaster?.Api != null)
                services.AddSingleton(RestService.For<IBitcoinMasterOnlineUserApi>(appSettings.GameServers.BitcoinMaster.Api, refitSettings));
            
            if (appSettings.GameServers.CasinoHoldemPoker?.Api != null)
                services.AddSingleton(RestService.For<ICasinoHoldemPokerOnlineUserApi>(appSettings.GameServers.CasinoHoldemPoker.Api, refitSettings));
            
            if (appSettings.GameServers.HiLo?.Api != null)
                services.AddSingleton(RestService.For<IHiLoApi>(appSettings.GameServers.HiLo.Api, refitSettings));
            
            if (appSettings.GameServers.TexasHoldemPoker?.Api != null)
                services.AddSingleton(RestService.For<ITexasHoldemPokerOnlineUserApi>(appSettings.GameServers.TexasHoldemPoker.Api, refitSettings));
            
            if (appSettings.GameServers.TreasureIsland?.Api != null)
                services.AddSingleton(RestService.For<ITreasureIslandOnlineUserApi>(appSettings.GameServers.TreasureIsland.Api, refitSettings));
            
            if (appSettings.GameServers.BlockchainBaccarat?.Api != null)
                services.AddSingleton(RestService.For<IBlockchainBaccaratOnlineUserApi>(appSettings.GameServers.BlockchainBaccarat.Api, refitSettings));
            
            if (appSettings.GameServers.MegaKeno?.Api != null)
                services.AddSingleton(RestService.For<IMegaKenoOnlineUserApi>(appSettings.GameServers.MegaKeno.Api, refitSettings));
            
            if (appSettings.GameServers.UnbalancedBigSmall1?.Api != null)
                services.AddSingleton(RestService.For<IUnbalancedBigSmallOnlineUserApi>(appSettings.GameServers.UnbalancedBigSmall1.Api, refitSettings));
            
            if (appSettings.GameServers.MegaKeno2?.Api != null)
                services.AddSingleton(RestService.For<IMegaKeno2OnlineUserApi>(appSettings.GameServers.MegaKeno2.Api, refitSettings));
            
            if (appSettings.GameServers.FruitSlot?.Api != null)
                services.AddSingleton(RestService.For<IFruitSlotOnlineUserApi>(appSettings.GameServers.FruitSlot.Api, refitSettings));
            
            if (appSettings.GameServers.FanTan?.Api != null)
                services.AddSingleton(RestService.For<IFanTanOnlineUserApi>(appSettings.GameServers.FanTan.Api, refitSettings));
            
            if (appSettings.GameServers.WinThreeCards?.Api != null)
                services.AddSingleton(RestService.For<IWinThreeCardsOnlineUserApi>(appSettings.GameServers.WinThreeCards.Api, refitSettings));
            
            if (appSettings.GameServers.DragonTigerFight?.Api != null)
                services.AddSingleton(RestService.For<IDragonTigerFightOnlineUserApi>(appSettings.GameServers.DragonTigerFight.Api, refitSettings));
            
            if (appSettings.GameServers.Pontoon?.Api != null)
                services.AddSingleton(RestService.For<IPontoonOnlineUserApi>(appSettings.GameServers.Pontoon.Api, refitSettings));
            
            if (appSettings.GameServers.ColorGame?.Api != null)
                services.AddSingleton(RestService.For<IColorGameOnlineUserApi>(appSettings.GameServers.ColorGame.Api, refitSettings));
            
            if (appSettings.GameServers.BandarQiuQiu?.Api != null)
                services.AddSingleton(RestService.For<IBandarQiuQiuOnlineUserApi>(appSettings.GameServers.BandarQiuQiu.Api, refitSettings));
            
            if (appSettings.GameServers.ChineseBlackjack?.Api != null)
                services.AddSingleton(RestService.For<IChineseBlackjackOnlineUserApi>(appSettings.GameServers.ChineseBlackjack.Api, refitSettings));
            
            if (appSettings.GameServers.FootballDice?.Api != null)
                services.AddSingleton(RestService.For<IFootballDiceOnlineUserApi>(appSettings.GameServers.FootballDice.Api, refitSettings));
            
            if (appSettings.GameServers.FootballSlot?.Api != null)
                services.AddSingleton(RestService.For<IFootballSlotOnlineUserApi>(appSettings.GameServers.FootballSlot.Api, refitSettings));
            
            if (appSettings.GameServers.CoinToss?.Api != null)
                services.AddSingleton(RestService.For<ICoinTossOnlineUserApi>(appSettings.GameServers.CoinToss.Api, refitSettings));
            
            if (appSettings.GameServers.SeDie3?.Api != null)
                services.AddSingleton(RestService.For<ISeDie3OnlineUserApi>(appSettings.GameServers.SeDie3.Api, refitSettings));
            
            if (appSettings.GameServers.KaraKrus?.Api != null)
                services.AddSingleton(RestService.For<IKaraKrusOnlineUserApi>(appSettings.GameServers.KaraKrus.Api, refitSettings));
            
            if (appSettings.GameServers.FootballCoinToss?.Api != null)
                services.AddSingleton(RestService.For<IFootballCoinTossOnlineUserApi>(appSettings.GameServers.FootballCoinToss.Api, refitSettings));
            
            if (appSettings.GameServers.SevenUpDown?.Api != null)
                services.AddSingleton(RestService.For<ISevenUpDownOnlineUserApi>(appSettings.GameServers.SevenUpDown.Api, refitSettings));
            
            if (appSettings.GameServers.MarbleFootball?.Api != null)
                services.AddSingleton(RestService.For<IMarbleFootballOnlineUserApi>(appSettings.GameServers.MarbleFootball.Api, refitSettings));
            services.AddSingleton(RestService.For<IMarbleFootball2OnlineUserApi>(appSettings.GameServers.MarbleFootball2.Api, refitSettings));
        }

        private static void AddOnlineUserApiGN(IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            if (appSettings.GameServers.BacBall1?.Api != null)
                services.AddSingleton(RestService.For<IBacBall1OnlineUserApi>(appSettings.GameServers.BacBall1.Api, refitSettings));
            
            if (appSettings.GameServers.BacBall2?.Api != null)
                services.AddSingleton(RestService.For<IBacBall2OnlineUserApi>(appSettings.GameServers.BacBall2.Api, refitSettings));
            
            if (appSettings.GameServers.LuckyNext1?.Api != null)
                services.AddSingleton(RestService.For<ILuckyNext1OnlineUserApi>(appSettings.GameServers.LuckyNext1.Api, refitSettings));
            
            if (appSettings.GameServers.LuckyNext2?.Api != null)
                services.AddSingleton(RestService.For<ILuckyNext2OnlineUserApi>(appSettings.GameServers.LuckyNext2.Api, refitSettings));
            
            if (appSettings.GameServers.SeBall1?.Api != null)
                services.AddSingleton(RestService.For<ISeBall1OnlineUserApi>(appSettings.GameServers.SeBall1.Api, refitSettings));
            
            if (appSettings.GameServers.LuckyThree?.Api != null)
                services.AddSingleton(RestService.For<ILuckyThreeOnlineUserApi>(appSettings.GameServers.LuckyThree.Api, refitSettings));
            
            if (appSettings.GameServers.SicBall?.Api != null)
                services.AddSingleton(RestService.For<ISicBallOnlineUserApi>(appSettings.GameServers.SicBall.Api, refitSettings));
        }

        private static void AddSlotServices(this IServiceCollection services)
        {
            services.AddScoped<ISlotBoardApiFactory, SlotBoardApiFactory>();
            services.AddScoped<ICascadingResultApiFactory, CascadingResultApiFactory>();
        }

        private static void AddHoldemPokerServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<IHoldemPokerApiFactory, HoldemPokerApiFactory>();
            if (appSettings.SystemType == SystemType.SC1 || appSettings.SystemType == SystemType.VG2 || appSettings.SystemType == SystemType.HD)
            {
                if (appSettings.GameServers.CasinoHoldemPoker?.Api != null)
                    services.AddSingleton(RestService.For<ICasinoHoldemPokerApi>(appSettings.GameServers.CasinoHoldemPoker.Api, refitSettings));
                
                if (appSettings.GameServers.TexasHoldemPoker?.Api != null)
                    services.AddSingleton(RestService.For<ITexasHoldemPokerApi>(appSettings.GameServers.TexasHoldemPoker.Api, refitSettings));
            }
        }

        private static void AddPontoonServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<IPontoonApiFactory, PontoonApiFactory>();
            if (appSettings.SystemType == SystemType.SC1 || appSettings.SystemType == SystemType.VG2 || appSettings.SystemType == SystemType.HD)
            {
                if (appSettings.GameServers.Pontoon?.Api != null)
                    services.AddSingleton(RestService.For<IPontoonApi>(appSettings.GameServers.Pontoon.Api, refitSettings));
            }
        }

        private static void AddCrashGameServices(this IServiceCollection services)
        {
            services.AddScoped<ICrashGameApiFactory, CrashGameApiFactory>();
            services.AddScoped<ICrashTurnBaseApiFactory, CrashTurnBaseApiFactory>();
        }

        private static void AddChineseBlackjackServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<IChineseBlackjackApiFactory, ChineseBlackjackApiFactory>();
            if (appSettings.SystemType == SystemType.SC1 || appSettings.SystemType == SystemType.VG2 || appSettings.SystemType == SystemType.HD)
            {
                if (appSettings.GameServers.ChineseBlackjack?.Api != null)
                    services.AddSingleton(RestService.For<IChineseBlackjackApi>(appSettings.GameServers.ChineseBlackjack.Api, refitSettings));
            }
        }

        private static void AddFootballDiceServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<IFootballDiceApiFactory, FootballDiceApiFactory>();
            if (appSettings.SystemType == SystemType.SC1 || appSettings.SystemType == SystemType.VG2 || appSettings.SystemType == SystemType.HD)
            {
                if (appSettings.GameServers.FootballDice?.Api != null)
                    services.AddSingleton(RestService.For<IFootballDiceApi>(appSettings.GameServers.FootballDice.Api, refitSettings));
            }
        }

        private static void AddBaccaratXServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            if (appSettings.GameServers.BaccaratX?.Api != null)
            {
                services.AddSingleton(RestService.For<IBaccaratXApi>(appSettings.GameServers.BaccaratX.Api, refitSettings));
                services.AddSingleton(RestService.For<IBaccaratXOnlineUserApi>(appSettings.GameServers.BaccaratX.Api, refitSettings));
            }
        }

        private static void AddChatBotServices(this IServiceCollection services, IAppSettings appSettings)
        {
            if (appSettings.ChatBotApiSettings?.Url != null)
            {
                HttpClient client = InitHttpClient(appSettings.ChatBotApiSettings.Url, appSettings.ChatBotApiSettings.Proxy);
                client.Timeout = TimeSpan.FromSeconds(appSettings.ChatBotApiSettings.TimeoutInSeconds);

                services.AddSingleton(RestService.For<IChatBotApi>(client));
                services.AddScoped<IChatBotService, ChatBotService>();
            }
        }

        private static void AddPublicService(this IServiceCollection services)
        {
            services.AddScoped<IGameConfigService, GameConfigService>();
        }

        private static void AddVoidRunningTicketServices(this IServiceCollection services)
        {
            services.AddScoped<IVoidRunningTicketApiFactory, VoidRunningTicketApiFactory>();
        }

        private static void AddCloudServices(this IServiceCollection services, IAppSettings appSettings)
        {
            if (!string.IsNullOrEmpty(appSettings.CloudSettings?.CloudAdminApiUrl))
            {
                var cloudClient = InitHttpClient(appSettings.CloudSettings.CloudAdminApiUrl, appSettings.CloudSettings.ProxyUrl);

                services.AddSingleton(RestService.For<IReportApi>(cloudClient));
                services.AddSingleton(RestService.For<ILogApi>(cloudClient));
                services.AddSingleton(RestService.For<IVoidGameRemoteApi>(cloudClient));
                services.AddSingleton(RestService.For<IPartnerRemoteApi>(cloudClient));
                services.AddSingleton(RestService.For<IGameManagementApi>(appSettings.CloudSettings.GameManagementUrl, SettingHelper.GetRefitSettings()));
            }
        }

        private static HttpClient InitHttpClient(string url, string proxy)
        {
            HttpClientHandler handler = null;

            if (!string.IsNullOrEmpty(proxy))
            {
                handler = new HttpClientHandler
                {
                    Proxy = new WebProxy(proxy),
                    UseProxy = true,
                };
            }

            var client = handler == null ? new HttpClient() : new HttpClient(handler);
            client.BaseAddress = new Uri(url);

            return client;
        }
    }
}