using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.Site.Features.Bot.ScheduleTasks;
using GamesAdmin.Site.Features.Dashboard.ScheduleTasks;
using Microsoft.Extensions.DependencyInjection;

namespace GamesAdmin.Site._Shared.Middlewares
{
    public static class SchedulerMiddleware
    {
        public static void AddScheduleTasks(this IScheduler scheduler)
        {
            scheduler
                .Schedule<ReviseBotBalance>()
                .EveryThirtyMinutes();

            scheduler
                .Schedule<DisableBot>()
                .Hourly();

            scheduler
                .Schedule<BanBot>()
                .Cron("0 0 1 * *");

            scheduler
                .Schedule<MonitorOnlineUsers>()
                .EveryMinute();
        }

        public static void AddScheduleTasks(this IServiceCollection services)
        {
            services.AddTransient<ReviseBotBalance>();
            services.AddTransient<DisableBot>();
            services.AddTransient<MonitorOnlineUsers>();
        }
    }
}
