using System;
using System.Net;
using System.Net.Http;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.BetDetails;
using GamesAdmin.Site.Features.Bot;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.ClientSites;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.DailySummary;
using GamesAdmin.Site.Features.Domain;
using GamesAdmin.Site.Features.ExchangeRates;
using GamesAdmin.Site.Features.GameRoundResult;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Features.Lobby;
using GamesAdmin.Site.Features.Notifications;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.Promotions;
using GamesAdmin.Site.Features.Report;
using GamesAdmin.Site.Features.RetrieveEndRound;
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

            services.AddScoped<IGeneralGameSettingService, GeneralGameSettingService>();
            services.AddSingleton(RestService.For<IGeneralGameSettingApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IGameSettingApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICascadingSlotsConfigValidator, CascadingSlotsConfigValidator>();
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

            services.AddSingleton(RestService.For<IBotApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IBotService, BotService>();

            services.AddSingleton(RestService.For<IRetrieveEndRoundApi>(appSettings.ResultPageConfiguration.BaseUrl));
            services.AddScoped<IRetrieveEndRoundService, RetrieveEndRoundService>();

            services.AddSingleton(RestService.For<IDailySummaryApi>(appSettings.SGApiUrl));
            services.AddScoped<IDailySummaryService, DailySummaryService>();

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();

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
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();

            services.AddSingleton(RestService.For<ILogApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IPromotionApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPromotionService, PromotionService>();

            services.AddSingleton(RestService.For<ILobbyApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ILobbyService, LobbyService>();

            services.AddSingleton(RestService.For<IDomainApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IDomainService, DomainService>();

            services.AddSingleton(RestService.For<IDomainApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IHmacHelpers, HmacHelpers>();

            services.AddReportServices();
            services.AddRoundHistoryServices(appSettings);
        }

        public static void AddRoundHistoryServices(this IServiceCollection services, IAppSettings appSettings)
        {
            var client = InitHttpClient(appSettings.ResultPageConfiguration.BaseUrl, appSettings.ResultPageConfiguration.ProxyUrl);
            services.AddSingleton(RestService.For<IGameRoundResultApi>(client));
            services.AddScoped<IGameRoundResultService, GameRoundResultService>();

            services.AddSingleton(RestService.For<IBetDetailsApi>(client));
            services.AddScoped<IBetDetailsService, BetDetailsService>();
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

            var client = (handler == null) ? new HttpClient() : new HttpClient(handler);
            client.BaseAddress = new Uri(url);

            return client;
        }
    }
}