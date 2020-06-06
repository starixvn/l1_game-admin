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
            var startTime = new TimeSpan(6, 1, 0);
            var endTime = new TimeSpan(21, 55, 0);
            var currentTime = DateTime.Now;

            if(currentTime.TimeOfDay.Subtract(startTime).TotalMinutes >= 1 && endTime.Subtract(currentTime.TimeOfDay).TotalMinutes >= 0)
            {
                var latestRound = await kenoRepository.GetLatestRoundAsync();
                var utcNow = currentTime.ToUniversalTime();

                var minutes  = (utcNow - latestRound?.Time)?.TotalMinutes;

                if (minutes >= 11)
                {
                    SentrySdk.CaptureMessage($"{currentTime} - The worker still hasnt recveived the latest round result from Vietlott!");
                }
            }
        }
    }
}