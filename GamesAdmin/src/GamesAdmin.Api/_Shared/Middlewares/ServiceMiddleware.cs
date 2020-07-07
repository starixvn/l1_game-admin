using GamesAdmin.Api.Accounts;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.GameRound;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.ProVietKeno;
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
            services.AddScoped<IProVietKenoService, ProVietKenoService>();

            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IBotService, BotService>();

            services.AddSingleton<IKenoReport, KenoProVietReport>();

            services.AddScoped<IKenoReportFactory, KenoReportFactory>();

            services.AddScoped<KenoProVietReport>()
                        .AddScoped<IKenoReport, KenoProVietReport>(s => s.GetService<KenoProVietReport>());

            services.AddScoped<KenoMaxReport>()
                        .AddScoped<IKenoReport, KenoMaxReport>(s => s.GetService<KenoMaxReport>());

            services.AddScoped<KenoMax2Report>()
                        .AddScoped<IKenoReport, KenoMax2Report>(s => s.GetService<KenoMax2Report>());
        }
    }
}
