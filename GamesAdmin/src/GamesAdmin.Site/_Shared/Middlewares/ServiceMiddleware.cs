using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.Bot;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.ClientSites;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.ExchangeRates;
using GamesAdmin.Site.Features.GameRoundResult;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Features.Notifications;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.ProVietKeno;
using GamesAdmin.Site.Features.Report;
using GamesAdmin.Site.Features.ResultMonitor;
using GamesAdmin.Site.Features.UmSchedule;
using GamesAdmin.Site.Features.Users;
using GamesAdmin.Site.Features.VoidGame;
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

            services.AddSingleton(RestService.For<IGameUserApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IGameUserService, GameUserService>();

            services.AddSingleton(RestService.For<IUserApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton(RestService.For<IPartnerApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPartnerService, PartnerService>();

            services.AddSingleton(RestService.For<IClientSiteApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IClientSiteService, ClientSiteService>();

            services.AddSingleton(RestService.For<IReportApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton(RestService.For<IUmApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUmService, UmService>();

            services.AddSingleton(RestService.For<INotificationApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<INotificationService, NotificationService>();

            services.AddSingleton(RestService.For<IProVietKenoApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IProVietKenoService, ProVietKenoService>();

            services.AddSingleton(RestService.For<IBotApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IBotService, BotService>();

            services.AddSingleton(RestService.For<IGameRoundResultApi>(appSettings.ResultPageConfiguration.BaseUrl));
            services.AddScoped<IGameRoundResultService, GameRoundResultService>();

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();
            services.AddSingleton(RestService.For<IResultMonitorApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IResultMonitorService, ResultMonitorService>();

            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton(RestService.For<IGlobalConfigApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<ICurrencyApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICurrencyService, CurrencyService>();

            services.AddSingleton(RestService.For<ICurrencyGroupApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICurrencyGroupService, CurrencyGroupService>();

            services.AddSingleton(RestService.For<IGroupCurrencyGameApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICurrencyGroupGameService, CurrencyGroupGameService>();

            services.AddSingleton(RestService.For<IChipApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IChipService, ChipService>();

            services.AddSingleton(RestService.For<IExchangeRateApi>(appSettings.ApiHost, refitSettings));
        }
    }
}