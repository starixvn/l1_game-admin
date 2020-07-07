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
               .EveryMinute()
               .When(CheckingLatestKenoRoundTask.CanRun);

            scheduler
                .Schedule<CrawlingKenoRoundTask>()
                .EveryMinute()
                .When(CrawlingKenoRoundTask.CanRun);
        }
    }
}