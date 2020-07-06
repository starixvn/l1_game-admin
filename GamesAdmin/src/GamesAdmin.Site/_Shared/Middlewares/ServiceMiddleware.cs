using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.IdentityServer;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Account;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.Bot;
using GamesAdmin.Site.Features.ClientSites;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Notifications;
using GamesAdmin.Site.Features.ProVietKeno;
using GamesAdmin.Site.Features.Report;
using GamesAdmin.Site.Features.UmSchedule;
using GamesAdmin.Site.Features.Users;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GamesAdmin.Site._Shared.Middlewares
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

            services.AddSingleton(RestService.For<IGameSettingApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IGameSettingService, GameSettingService>();

            services.AddSingleton(RestService.For<IAccountApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IAccountService, AccountService>();

            services.AddSingleton(RestService.For<IUserApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton(RestService.For<IClientSiteApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IClientSiteService, ClientSiteService>();

            services.AddSingleton(RestService.For<IReportApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton(RestService.For<IUmApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUmService, UmService>();

            services.AddSingleton(RestService.For<INotificationApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<INotificationService, NotificationService>();

            services.AddSingleton(RestService.For<IGameServerApi>(appSettings.GameServerUrl, refitSettings));
            services.AddScoped<IGameServerService, GameServerService>();

            services.AddSingleton(RestService.For<IProVietKenoApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IProVietKenoService, ProVietKenoService>();

            services.AddSingleton(RestService.For<IBotApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IBotService, BotService>();

            services.AddSingleton<ICacheManager, CacheManager>();
        }
    }
}