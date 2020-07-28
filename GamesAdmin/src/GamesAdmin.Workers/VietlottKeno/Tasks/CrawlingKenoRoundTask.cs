using Coravel.Invocable;
using GamesAdmin.Core.Models;
using GamesAdmin.Workers._Shared.Configurations;
using GamesAdmin.Workers.VietlottKeno;
using GamesAdmin.Workers.VietlottKeno.DTOs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sentry;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GamesAdmin.VietlottKeno.Tasks
{
    public class CrawlingKenoRoundTask : IInvocable
    {
        private readonly IKenoService kenoService;
        private readonly ISentryClient sentryClient;
        private readonly VietlottKenoConfiguration configuration;

        public CrawlingKenoRoundTask(IKenoService kenoService, IOptions<VietlottKenoConfiguration> options, ISentryClient sentryClient)
        {
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
            var uri = new Uri(configuration.WebUrl);
            var kenoResult = await Task.Run(() => CrawlKenoResult(uri));

            if (kenoResult == default)
            {
                return;
            }

            var currentLatestRound = await kenoService.GetLatestRoundAsync();

            if (IsNotHaveLatestResult(kenoResult.RoundId, currentLatestRound))
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

        private static bool IsNotHaveLatestResult(long crawlRoundId, ProVietKenoRound currentLatestRound)
        => currentLatestRound == null
           || (currentLatestRound.Result == null && currentLatestRound.Number == crawlRoundId)
           || currentLatestRound.Number < crawlRoundId;


        private KenoResultDto CrawlKenoResult(Uri url)
        {
            try
            {
                var responseFromServer = string.Empty;
                var request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;

                var response = request.GetResponse();

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();

                var regex = new Regex(@"\""lastResult"": {([^}]+)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matches = regex.Matches(responseFromServer);
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

            return Task.FromResult(currentTime.Subtract(startTimeOfDay).TotalMinutes >= 12
                         || endTimeOfDay.Subtract(currentTime).TotalMinutes >= 0);
        }
    }
}