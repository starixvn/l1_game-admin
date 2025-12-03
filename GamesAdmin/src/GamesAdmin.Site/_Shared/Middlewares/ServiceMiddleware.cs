using System.Globalization;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features;
using GamesAdmin.Site.Features.Authentication;
using GamesAdmin.Site.Features.BetDetails;
using GamesAdmin.Site.Features.Bot;
using GamesAdmin.Site.Features.BuyFreeSpins;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.ClientSites;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroupGames;
using GamesAdmin.Site.Features.CurrencyGroups;
using GamesAdmin.Site.Features.DailySummary;
using GamesAdmin.Site.Features.Domain;
using GamesAdmin.Site.Features.ExchangeRates;
using GamesAdmin.Site.Features.GameGroupManagement;
using GamesAdmin.Site.Features.GameRoundResult;
using GamesAdmin.Site.Features.GameSettings;
using GamesAdmin.Site.Features.GameUsers;
using GamesAdmin.Site.Features.InternalTestAccount;
using GamesAdmin.Site.Features.Lobby;
using GamesAdmin.Site.Features.Notifications;
using GamesAdmin.Site.Features.OperateSettings;
using GamesAdmin.Site.Features.Partners;
using GamesAdmin.Site.Features.PlayerChainResultsManagement.Services;
using GamesAdmin.Site.Features.PlayerInfo;
using GamesAdmin.Site.Features.Promotions;
using GamesAdmin.Site.Features.Report;
using GamesAdmin.Site.Features.RetrieveEndRound;
using GamesAdmin.Site.Features.UmSchedule;
using GamesAdmin.Site.Features.Users;
using GamesAdmin.Site.Features.VoidGame;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    Culture = CultureInfo.InvariantCulture
                }),
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken,
            };

            services.AddScoped<IGeneralGameSettingService, GeneralGameSettingService>();
            services.AddSingleton(RestService.For<IGeneralGameSettingApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IGameSettingApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICascadingSlotsConfigValidator, CascadingSlotsConfigValidator>();
            services.AddScoped<IGameSettingService, GameSettingService>();

            services.AddSingleton(RestService.For<IShootingBlitzRtpApi>(appSettings.ApiHost, refitSettings));
            services.AddSingleton(RestService.For<IShootingBlitzTeamApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IGameUserApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IGameUserService, GameUserService>();

            services.AddSingleton(RestService.For<IUserApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton(RestService.For<IPartnerApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPartnerService, PartnerService>();

            services.AddSingleton(RestService.For<IClientSiteApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IClientSiteService, ClientSiteService>();

            services.AddSingleton(RestService.For<IReportApi>(appSettings.ApiHost, refitSettings));
            services.AddSingleton(RestService.For<IRemoteReportApi>(appSettings.ResultPageConfiguration.BaseUrl, refitSettings));
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton(RestService.For<IUmApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IUmService, UmService>();

            services.AddSingleton(RestService.For<INotificationApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<INotificationService, NotificationService>();

            services.AddSingleton(RestService.For<IBotApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IBotService, BotService>();

            services.AddSingleton(RestService.For<IGameRoundResultApi>(appSettings.ResultPageConfiguration.BaseUrl));
            services.AddScoped<IGameRoundResultService, GameRoundResultService>();

            services.AddSingleton(RestService.For<IBetDetailsApi>(appSettings.ResultPageConfiguration.BaseUrl));
            services.AddScoped<IBetDetailsService, BetDetailsService>();

            services.AddSingleton(RestService.For<IRetrieveEndRoundApi>(appSettings.SGApiUrl));
            services.AddScoped<IRetrieveEndRoundService, RetrieveEndRoundService>();

            services.AddSingleton(RestService.For<IDailySummaryApi>(appSettings.SGApiUrl));
            services.AddScoped<IDailySummaryService, DailySummaryService>();

            services.AddSingleton(RestService.For<IVoidGameApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IVoidGameService, VoidGameService>();

            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton(RestService.For<IGlobalConfigApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<ICurrencyApi>(appSettings.ApiHost, refitSettings));
            
            services.AddSingleton(RestService.For<IPlayerChainResultApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPlayerChainResultService, PlayerChainResultService>();
            services.AddScoped<ICurrencyService, CurrencyService>();

            services.AddSingleton(RestService.For<ICurrencyGroupApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICurrencyGroupService, CurrencyGroupService>();

            services.AddSingleton(RestService.For<IGroupCurrencyGameApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ICurrencyGroupGameService, CurrencyGroupGameService>();

            services.AddSingleton(RestService.For<IChipApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IChipService, ChipService>();

            services.AddSingleton(RestService.For<IBuyFreeSpinApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IExchangeRateApi>(appSettings.ApiHost, refitSettings));
            services.AddSingleton<IExchangeRateService, ExchangeRateService>();

            services.AddSingleton(RestService.For<ILogApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IPromotionApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromotionCampaignService, PromotionCampaignService>();

            services.AddSingleton(RestService.For<ILobbyApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<ILobbyService, LobbyService>();

            services.AddSingleton(RestService.For<IDomainApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IDomainService, DomainService>();

            services.AddScoped(x => RestService.For<IGameGroupManagementApi>(appSettings.ApiHost, refitSettings));

            services.AddSingleton(RestService.For<IInternalTestAccountApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IInternalTestAccountService, InternalTestAccountService>();

            services.AddSingleton(RestService.For<IPlayerInfoApi>(appSettings.ApiHost, refitSettings));
            services.AddScoped<IPlayerInfoService, PlayerInfoService>();

            services.AddReportServices();

            services.RegisterOperatorSettings(appSettings, refitSettings);
        }
    }
}