using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.VietlottKeno.Tasks;

namespace GamesAdmin.Workers._Shared
{
    public static class Schedulers
    {
        public static void AddScheduleTasks(this IScheduler scheduler)
        {
            scheduler
               .Schedule<CheckingLatestKenoRoundTask>()
               .Cron("* 6-21 * * *");
        }
    }
}