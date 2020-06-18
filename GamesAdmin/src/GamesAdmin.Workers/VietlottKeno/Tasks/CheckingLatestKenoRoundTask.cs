using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using Sentry;
using Sentry.Protocol;

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
            var firstRoundTime = new TimeSpan(23, 10, 0);
            var finalRoundTime = new TimeSpan(14, 55, 0);
            var currentTime = DateTime.UtcNow;

            if(currentTime.TimeOfDay.Subtract(firstRoundTime).TotalMinutes >= 2 || finalRoundTime.Subtract(currentTime.TimeOfDay).TotalMinutes >= 0)
            {
                var latestRound = await kenoRepository.GetLatestRoundAsync();

                var minutes  = (currentTime - latestRound?.Time)?.TotalMinutes;

                if (minutes >= 11)
                {
                    SentrySdk.CaptureMessage($"{currentTime} - The worker still hasnt recveived the latest round result from Vietlott!", SentryLevel.Fatal);
                }
            }
        }
    }
}