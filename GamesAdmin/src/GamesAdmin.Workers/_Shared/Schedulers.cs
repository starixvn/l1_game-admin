using Coravel.Scheduling.Schedule.Interfaces;
using GamesAdmin.Workers.Bots.Tasks;

namespace GamesAdmin.Workers._Shared
{
    public static class Schedulers
    {
        public static void AddScheduleTasks(this IScheduler scheduler)
        {
            scheduler
                .Schedule<ReviseBotBalanceTask>()
                .EverySeconds(30);

            scheduler
                .Schedule<DisableBotTask>()
                .EverySeconds(30);
        }
    }
}
