using GamesAdmin.Api._Shared.Configurations;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.DailySummarize;
using GamesAdmin.Api.EndGameInfo;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameServers;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.GlobalConfigs;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.ProVietKeno;
using GamesAdmin.Api.Report;
using GamesAdmin.Api.Report.KenoReports;
using GamesAdmin.Api.UM;
using GamesAdmin.Api.Users;
using GamesAdmin.Api.VoidGame;
using GamesAdmin.Core.IdentityServer;
using Microsoft.Extensions.DependencyInjection;
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
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
            };

            services.AddScoped<IGameSettingsService, GameSettingsService>();
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

            services.AddScoped<IKenoReportFactory, KenoReportFactory>();
            services.AddScoped<KenoProVietReport>()
                        .AddScoped<IKenoReport, KenoProVietReport>(s => s.GetService<KenoProVietReport>());
            services.AddScoped<KenoMaxReport>()
                        .AddScoped<IKenoReport, KenoMaxReport>(s => s.GetService<KenoMaxReport>());
            services.AddScoped<KenoMax2Report>()
                        .AddScoped<IKenoReport, KenoMax2Report>(s => s.GetService<KenoMax2Report>());
            services.AddScoped<KenoMiniReport>()
                        .AddScoped<IKenoReport, KenoMiniReport>(s => s.GetService<KenoMiniReport>());
            services.AddScoped<KenoMini2Report>()
                        .AddScoped<IKenoReport, KenoMini2Report>(s => s.GetService<KenoMini2Report>());
            services.AddScoped<KenoEastReport>()
                        .AddScoped<IKenoReport, KenoEastReport>(s => s.GetService<KenoEastReport>());
            services.AddScoped<KenoWestReport>()
                        .AddScoped<IKenoReport, KenoWestReport>(s => s.GetService<KenoWestReport>());
            services.AddScoped<KenoSouthReport>()
                        .AddScoped<IKenoReport, KenoSouthReport>(s => s.GetService<KenoSouthReport>());
            services.AddScoped<KenoNorthReport>()
                        .AddScoped<IKenoReport, KenoNorthReport>(s => s.GetService<KenoNorthReport>());
            services.AddScoped<KenoProMaxReport>()
                        .AddScoped<IKenoReport, KenoProMaxReport>(s => s.GetService<KenoProMaxReport>());

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

            services.AddScoped<IOnlineUserApiFactory, OnlineUserApiFactory>();

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.SabaApiSettings.Url, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();
            services.AddScoped<ITicketHandler, TicketHandler>();
            services.AddScoped<IVoidTicketLogger, VoidTicketLogger>();
            AddSlotServices(services, appSettings, refitSettings);
        }

        private static void AddSlotServices(this IServiceCollection services, IAppSettings appSettings, RefitSettings refitSettings) 
        {
            services.AddScoped<ISlotBoardApiFactory, SlotBoardApiFactory>();
            services.AddSingleton(RestService.For<IFourGuardiansBoardApi>(appSettings.GameServers.FourGuardians.Api, refitSettings));
        }
    }
}
