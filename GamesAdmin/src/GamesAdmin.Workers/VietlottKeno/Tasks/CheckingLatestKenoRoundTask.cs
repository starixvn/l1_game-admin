using System;
using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Workers.VietlottKeno;
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
            var currentTime = DateTime.UtcNow;
            var latestRound = await kenoRepository.GetLatestRoundAsync();

            var minutes = (currentTime - latestRound?.Time)?.TotalMinutes;

            if (minutes >= 1)
            {
                SentrySdk.CaptureMessage($"{currentTime} - The worker still hasnt recveived the latest round result from Vietlott!", SentryLevel.Fatal);
            }
        }

        public static Task<bool> CanRun()
        {
            var startTimeOfDay = VietlottConstants.StartTimeOfDayUtc;
            var endTimeOfDay = VietlottConstants.EndTimeOfDayUtc;
            var currentTime = DateTime.UtcNow.TimeOfDay;

            return Task.FromResult(currentTime.Subtract(startTimeOfDay).TotalMinutes >= 12 
                || endTimeOfDay.Subtract(currentTime).TotalMinutes >= 0);
       }
    }
}