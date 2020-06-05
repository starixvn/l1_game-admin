using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.VietlottKeno.Tasks;
using GamesAdmin.Workers.Bots.Tasks;

namespace GamesAdmin.Workers._Shared
{
    public static class Schedulers
    {
        public static void AddScheduleTasks(this IScheduler scheduler)
        {
            scheduler
                .Schedule<ReviseBotBalanceTask>()
                .EveryThirtyMinutes();

            scheduler
                .Schedule<DisableBotTask>()
                .Hourly();

            //scheduler
            //   .Schedule<CheckingLatestKenoRoundTask>()
            //   .EveryMinute();
        }
    }
}