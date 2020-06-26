using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.Site.Features.Bot.ScheduleTasks;
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
        }

        public static void AddScheduleTasks(this IServiceCollection services)
        {
            services.AddTransient<ReviseBotBalance>();
            services.AddTransient<DisableBot>();
        }
    }
}
