using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.IdentityServer;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Account;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.ClientSites;
using GamesAdmin.Site.Features.Dashboard;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.Report;
using GamesAdmin.Site.Features.Users;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GamesAdmin.Site._Shared.Midlewares
{
    public static class ServiceMiddleware
    {
        public static void AddServices(this IServiceCollection services, IAppSettings appSettings)
        {
            //IdentityServerManager.Initialize(appSettings.IdentityServerAuth);

            var refitSettings = new RefitSettings
            {
                //AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken
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
           
            services.AddSingleton(RestService.For<IStatisticApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton(RestService.For<IGameServerApi>(appSettings.GameServerUrl));
            services.AddScoped<IGameServerService, GameServerService>();
        }
    }
}
