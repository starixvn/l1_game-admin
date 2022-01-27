using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ApiLogs;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Api.Chips;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroupGames;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.DailySummarize;
using GamesAdmin.Api.EndGameInfo;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameServers;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.GlobalConfigs;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Partners;
using GamesAdmin.Api.ProVietKeno;
using GamesAdmin.Api.Report;
using GamesAdmin.Api.UM;
using GamesAdmin.Api.Users;
using GamesAdmin.Api.VoidGame;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class ServiceMiddleware
    {
        public static void AddServices(this IServiceCollection services, IAppSettings appSettings)
        {
            IdentityServerManager.Initialize(appSettings.IdentityServerAuth);

            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
            };

            services.AddScoped<IGameSettingsService, GameSettingsService>();
            services.AddScoped<ICurrencyGroupGameService, CurrencyGroupGameService>();
            services.AddScoped<IUMService, UMService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientSiteSiteSerivce, ClientSiteSerivce>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IProVietKenoService, ProVietKenoService>();
            services.AddScoped<IDailySummaryService, DailySummaryService>();
            services.AddScoped<IGameUserService, GameUserService>();
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IGlobalConfigService, GlobalConfigService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICurrencyGroupService, CurrencyGroupService>();
            services.AddScoped<IChipService, ChipService>();

            services.AddScoped<ISlotGameReportFactory, SlotGameReportFactory>();
            services.AddScoped<FourGuardiansReport>().AddScoped<ISlotGameReport, FourGuardiansReport>(s => s.GetService<FourGuardiansReport>());
            services.AddScoped<HeroesSlotReport>().AddScoped<ISlotGameReport, HeroesSlotReport>(s => s.GetService<HeroesSlotReport>());

            services.AddScoped<IEndGameInfoServiceFactory, EndGameInfoServiceFactory>();
            services.AddScoped<BigSmallEndGameInfoService>();
            services.AddScoped<BigSmallTurboEndGameInfoService>();
            services.AddScoped<OddEvenEndGameInfoService>();
            services.AddScoped<OddEvenTurboEndGameInfoService>();
            services.AddScoped<FishPrawnCrabEndGameInfoService>();
            services.AddScoped<SeDieEndGameInfoService>();
            services.AddScoped<MiniSeDieEndGameInfoService>();
            services.AddScoped<DragonTigerEndGameInfoService>();
            services.AddScoped<BaccaratEndGameInfoService>();
            services.AddScoped<VipBaccaratEndGameInfoService>();
            services.AddScoped<KenoProMaxEndGameInfoService>();
            services.AddScoped<KenoProVietEndGameInfoService>();
            services.AddScoped<KenoMaxEndGameInfoService>();
            services.AddScoped<KenoMax2EndGameInfoService>();
            services.AddScoped<KenoMiniEndGameInfoService>();
            services.AddScoped<KenoMini2EndGameInfoService>();
            services.AddScoped<KenoEastEndGameInfoService>();
            services.AddScoped<KenoWestEndGameInfoService>();
            services.AddScoped<KenoSouthEndGameInfoService>();
            services.AddScoped<KenoNorthEndGameInfoService>();
            services.AddScoped<RouletteEndGameInfoService>();
            services.AddScoped<VipSeDieEndGameInfoService>();
            services.AddScoped<BlackjackEndGameInfoService>();
            services.AddScoped<SpecialSeDieEndGameInfoService>();
            services.AddScoped<VipFishPrawnCrabEndGameInfoService>();
            services.AddScoped<SicBoEndGameInfoService>();
            services.AddScoped<VipSeDieWarEndGameInfoService>();
            services.AddScoped<FourGuardiansEndGameInfoService>();
            services.AddScoped<LotteryEndGameInfoService>();
            services.AddScoped<ThreeCardsEndGameInfoService>();
            services.AddScoped<HeroesSlotEndGameInfoService>();
            services.AddScoped<BitcoinMasterEndGameInfoService>();
            services.AddScoped(typeof(KenoMatchEndGameInfoService<,>));
            services.AddScoped(typeof(KenoPowerEndGameInfoService<,>));
            services.AddScoped<CasinoHoldemPokerEndGameInfoService>();
            services.AddScoped<HiLoEndGameInfoService>();

            services.AddScoped<IGameServerService, GameServerService>();
            services.AddSingleton(RestService.For<IGameServerApi>(appSettings.GameServers.Main.Api, refitSettings));

            services.AddSingleton(RestService.For<IMainOnlineUserApi>(appSettings.GameServers.Main.Api, refitSettings));
            services.AddSingleton(RestService.For<ISedieOnlineUserApi>(appSettings.GameServers.Sedie.Api, refitSettings));
            services.AddSingleton(RestService.For<IVipSedieOnlineUserApi>(appSettings.GameServers.VipSedie.Api, refitSettings));
            services.AddSingleton(RestService.For<ISpecialSedieOnlineUserApi>(appSettings.GameServers.SpecialSedie.Api, refitSettings));
            services.AddSingleton(RestService.For<IBlackjackOnlineUserApi>(appSettings.GameServers.Blackjack.Api, refitSettings));
            services.AddSingleton(RestService.For<IVipFishPrawnCrabOnlineUserApi>(appSettings.GameServers.VipFishPrawnCrab.Api, refitSettings));
            services.AddSingleton(RestService.For<IVipBaccaratOnlineUserApi>(appSettings.GameServers.VipBaccarat.Api, refitSettings));
            services.AddSingleton(RestService.For<ISicBoOnlineUserApi>(appSettings.GameServers.SicBo.Api, refitSettings));
            services.AddSingleton(RestService.For<IVipSedieWarOnlineUserApi>(appSettings.GameServers.VipSedieWar.Api, refitSettings));
            services.AddSingleton(RestService.For<IFourGuardiansOnlineUserApi>(appSettings.GameServers.FourGuardians.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoProMaxOnlineUserApi>(appSettings.GameServers.KenoProMax.Api, refitSettings));
            services.AddSingleton(RestService.For<IMiniSeDieOnlineUserApi>(appSettings.GameServers.MiniSeDie.Api, refitSettings));
            services.AddSingleton(RestService.For<IRouletteOnlineUserApi>(appSettings.GameServers.Roulette.Api, refitSettings));
            services.AddSingleton(RestService.For<IBaccaratOnlineUserApi>(appSettings.GameServers.Baccarat.Api, refitSettings));
            services.AddSingleton(RestService.For<IFishPrawnCrabOnlineUserApi>(appSettings.GameServers.FishPrawnCrab.Api, refitSettings));
            services.AddSingleton(RestService.For<IDragonTigerOnlineUserApi>(appSettings.GameServers.DragonTiger.Api, refitSettings));
            services.AddSingleton(RestService.For<IBigSmallOnlineUserApi>(appSettings.GameServers.BigSmall.Api, refitSettings));
            services.AddSingleton(RestService.For<ITurboBigSmallOnlineUserApi>(appSettings.GameServers.TurboBigSmall.Api, refitSettings));
            services.AddSingleton(RestService.For<ITurboOddEvenOnlineUserApi>(appSettings.GameServers.TurboOddEven.Api, refitSettings));
            services.AddSingleton(RestService.For<ILotteryOnlineUserApi>(appSettings.GameServers.Lottery.Api, refitSettings));
            services.AddSingleton(RestService.For<IThreeCardsOnlineUserApi>(appSettings.GameServers.ThreeCards.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoMatchOnlineUserApi>(appSettings.GameServers.KenoMatch1.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoPowerOnlineUserApi>(appSettings.GameServers.KenoPower1.Api, refitSettings));
            services.AddSingleton(RestService.For<IHeroesSlotOnlineUserApi>(appSettings.GameServers.HeroesSlot.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoMaxOnlineUserApi>(appSettings.GameServers.KenoMax.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoMax2OnlineUserApi>(appSettings.GameServers.KenoMax2.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoMiniOnlineUserApi>(appSettings.GameServers.KenoMini.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoMini2OnlineUserApi>(appSettings.GameServers.KenoMini2.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoEastOnlineUserApi>(appSettings.GameServers.KenoEast.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoWestOnlineUserApi>(appSettings.GameServers.KenoWest.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoSouthOnlineUserApi>(appSettings.GameServers.KenoSouth.Api, refitSettings));
            services.AddSingleton(RestService.For<IKenoNorthOnlineUserApi>(appSettings.GameServers.KenoNorth.Api, refitSettings));
            services.AddSingleton(RestService.For<IBitcoinMasterOnlineUserApi>(appSettings.GameServers.BitcoinMaster.Api, refitSettings));
            services.AddSingleton(RestService.For<ICasinoHoldemPokerOnlineUserApi>(appSettings.GameServers.CasinoHoldemPoker.Api, refitSettings));
            services.AddSingleton(RestService.For<IHiLoApi>(appSettings.GameServers.HiLo.Api, refitSettings));

            services.AddScoped<IOnlineUserApiFactory, OnlineUserApiFactory>();

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.SabaApiSettings.Url, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();
            services.AddScoped<ITicketHandler, TicketHandler>();
            services.AddScoped<IVoidTicketLogger, VoidTicketLogger>();
            AddSlotServices(services, appSettings, refitSettings);
            AddChatBotServices(services, appSettings);

            services.AddScoped<IReportServiceFactory, ReportServiceFactory>();
            services.AddScoped<SeDieReportService>();
            services.AddScoped<MiniSeDieReportService>();
            services.AddScoped<FishPrawnCrabReportService>();
            services.AddScoped<DragonTigerReportService>();
            services.AddScoped<BaccaratReportService>();
            services.AddScoped<RouletteReportService>();
            services.AddScoped<VipSeDieReportService>();
            services.AddScoped<BlackjackReportService>();
            services.AddScoped<SicBoReportService>();
            services.AddScoped<VipFishPrawnCrabReportService>();
            services.AddScoped<VipBaccaratReportService>();
            services.AddScoped<LotteryReportService>();
            services.AddScoped<ThreeCardsReportService>();
            services.AddScoped(typeof(KenoMatchReportService<,,>));
            services.AddScoped(typeof(KenoPowerReportService<,,>));
            services.AddScoped(typeof(MiniKenoReportService<,,>));
            services.AddScoped<VipSeDieWarReportService>();
            services.AddScoped<KenoProMaxReportService>();
            services.AddScoped<HiLoReportService>();

            services.InitApiLog();
        }

        private static void AddSlotServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings)
        {
            services.AddScoped<ISlotBoardApiFactory, SlotBoardApiFactory>();
            services.AddSingleton(RestService.For<IFourGuardiansBoardApi>(appSettings.GameServers.FourGuardians.Api, refitSettings));
            services.AddSingleton(RestService.For<IHeroesSlotBoardApi>(appSettings.GameServers.HeroesSlot.Api, refitSettings));
        }

        private static void AddChatBotServices(this IServiceCollection services, IAppSettings appSettings)
        {
            services.AddSingleton(RestService.For<IChatBotApi>(new HttpClient
            {
                BaseAddress = new Uri(appSettings.ChatBotApiSettings.Url),
                Timeout = TimeSpan.FromSeconds(appSettings.ChatBotApiSettings.TimeoutInSeconds)
            }));

            services.AddScoped<IChatBotService, ChatBotService>();
        }
    }
}