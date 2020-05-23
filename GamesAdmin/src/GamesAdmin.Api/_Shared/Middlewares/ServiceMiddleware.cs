using GamesAdmin.Api.Accounts;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.GameRound;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Report;
using GamesAdmin.Api.Statistic;
using GamesAdmin.Api.UM;
using GamesAdmin.Api.Users;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class ServiceMiddleware
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<IGameSettingsService, GameSettingsService>();
            services.AddScoped<IGameRoundService, GameRoundService>();
            services.AddScoped<IUMService, UMService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientSiteSiteSerivce, ClientSiteSerivce>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddSingleton<IAccountService, AccountService>();
        }
    }
}
