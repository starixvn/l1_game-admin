using Coravel.Invocable;
using GamesAdmin.Core.Models;
using GamesAdmin.Workers._Shared;
using GamesAdmin.Workers._Shared.Configurations;
using GamesAdmin.Workers.VietlottKeno;
using GamesAdmin.Workers.VietlottKeno.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sentry;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.VietlottKeno.Tasks
{
    public class CrawlingKenoRoundTask : IInvocable
    {
        private const int MaxMinutesBetweenRounds = 12;

        private readonly IKenoWebApi crawlApi;
        private readonly IKenoService kenoService;
        private readonly ISentryClient sentryClient;
        private readonly VietlottKenoConfiguration configuration;

        public CrawlingKenoRoundTask(
            IKenoWebApi crawlApi,
            IKenoService kenoService,
            IOptions<VietlottKenoConfiguration> options,
            ISentryClient sentryClient)
        {
            this.crawlApi = crawlApi;
            this.kenoService = kenoService;
            this.sentryClient = sentryClient;
            configuration = options.Value;
        }

        public async Task Invoke()
        {
            if (!configuration.EnableWebCrawling)
            {
                return;
            }

            var currentTime = DateTime.UtcNow;
            var kenoResult = await CrawlKenoResult();
            var currentLatestRound = await kenoService.GetLatestRoundAsync();

            if (kenoResult == default && IsWaitingResult(currentLatestRound))
            {
                sentryClient.CaptureMessage("Crawl keno from MinhChinh failed!", Sentry.Protocol.SentryLevel.Error);
                return;
            }

            if (kenoResult != null && IsNotHaveLatestResult(kenoResult.RoundId, currentLatestRound))
            {
                var result = kenoResult.Results.Select(x => byte.Parse(x)).OrderBy(x => x);

                var round = await kenoService.AddNewRoundResultAsync(
                    new CreateProVietKenoRound
                    {
                        Time = currentTime,
                        Number = kenoResult.RoundId,
                        Result = result
                    });

                await kenoService.AddRoundLogEntity(round.Number, round.Time, configuration.WebUrl);

                _ = Task.Run(() => kenoService.NotifyRoundResult(new[] { round }));
            }
        }

        private static bool IsWaitingResult(ProVietKenoRound currentLatestRound)
            => currentLatestRound == null
                || (currentLatestRound.Result == null && currentLatestRound.Time.AddMinutes(2) < DateTime.Now);

        private static bool IsNotHaveLatestResult(long crawlRoundId, ProVietKenoRound currentLatestRound)
        => currentLatestRound == null
           || (currentLatestRound.Result == null && currentLatestRound.Number == crawlRoundId)
           || currentLatestRound.Number < crawlRoundId;


        private async Task<KenoResultDto> CrawlKenoResult()
        {
            try
            {
                var response = await crawlApi.GetResult();

                var regex = new Regex(@"\""lastResult"": {([^}]+)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matches = regex.Matches(response);
                var rawResult = matches.FirstOrDefault()?.Value.Replace(@"""lastResult"": ", string.Empty).Trim();

                return string.IsNullOrWhiteSpace(rawResult)
                    ? default
                    : JsonConvert.DeserializeObject<KenoResultDto>(rawResult);
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
            }

            return default;
        }

        public static Task<bool> CanRun()
        {
            var startTimeOfDay = VietlottConstants.StartTimeOfDayUtc;
            var endTimeOfDay = VietlottConstants.EndTimeOfDayUtc;
            var currentTime = DateTime.UtcNow.TimeOfDay;

            return Task.FromResult(currentTime.Subtract(startTimeOfDay).TotalMinutes >= MaxMinutesBetweenRounds
                         || endTimeOfDay.Subtract(currentTime).TotalMinutes >= 0);
        }
    }
}