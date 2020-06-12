using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Sentry;

namespace GamesAdmin.VietlottKeno.Tasks
{
    public class CheckingLatestKenoRoundTask : IInvocable
    {
        private readonly IKenoRepository kenoRepository;
        public CheckingLatestKenoRoundTask(IKenoRepository kenoRepository)
        {
            this.kenoRepository = kenoRepository;
        }

        public async Task Invoke()
        {
            var startTime = new TimeSpan(23, 0, 0);
            var endTime = new TimeSpan(14, 55, 0);
            var currentTime = DateTime.UtcNow;

            if(currentTime.TimeOfDay.Subtract(startTime).TotalMinutes >= 2 || endTime.Subtract(currentTime.TimeOfDay).TotalMinutes >= 0)
            {
                var latestRound = await kenoRepository.GetLatestRoundAsync();

                var minutes  = (currentTime - latestRound?.Time)?.TotalMinutes;

                if (minutes >= 11)
                {
                    SentrySdk.CaptureMessage($"{currentTime} - The worker still hasnt recveived the latest round result from Vietlott!");
                }
            }
        }
    }
}