using GamesAdmin.Api.Accounts;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.GameRound;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.ProVietKeno;
using GamesAdmin.Api.Report;
using GamesAdmin.Api.UM;
using GamesAdmin.Api.Users;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Api._Shared.Middlewares
{
    public static class ServiceMiddleware
    {
        public static void AddServices(this IServiceCollection services)
        {
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
        }
    }
}
