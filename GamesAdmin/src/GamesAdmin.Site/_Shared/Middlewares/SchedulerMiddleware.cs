using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.Site.Features.Bot.ScheduleTasks;
using GamesAdmin.Site.Features.Dashboard.ScheduleTasks;
using GamesAdmin.Site.Features.Monitors.ScheduleTasks;
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
                .Schedule<MonitorOnlineUsers>()
                .EveryMinute();

            scheduler
                .Schedule<MonitorGameRoundTask>()
                .Cron("*/2 * * * *"); // runs every 2 minutes
        }

        public static void AddScheduleTasks(this IServiceCollection services)
        {
            services.AddTransient<ReviseBotBalance>();
            services.AddTransient<DisableBot>();
            services.AddTransient<MonitorOnlineUsers>();
            services.AddTransient<MonitorGameRoundTask>();
        }
    }
}
