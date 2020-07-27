using System;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Core.Models;
using GamesAdmin.Workers._Shared.Configurations;
using GamesAdmin.Workers.VietlottKeno;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Sentry;

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
            configuration = options.Value;
            this.sentryClient = sentryClient;
        }

        public async Task Invoke()
        {
            sentryClient.CaptureMessage($"{DateTime.Now} Invoke task crawling from {configuration.WebUrl}");

            if (!configuration.EnableWebCrawling) 
            {
                return;
            }

            var currentTime = DateTime.UtcNow;
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(configuration.WebUrl);

            var latestRoundNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@id = 'kq_ky']");
            var latestRoundId = long.Parse(latestRoundNode.InnerText.Remove(0, 1));

            sentryClient.CaptureMessage($"{DateTime.Now} Got result for round {latestRoundId} from {configuration.WebUrl}");

            var currentLatestRound = await kenoService.GetLatestRoundAsync();

            if (currentLatestRound == null || (currentLatestRound.Result == null && currentLatestRound.Number == latestRoundId) || currentLatestRound.Number < latestRoundId)
            {
                var kenoBallNodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='keno_ball']");
                var result = kenoBallNodes.Select(x => byte.Parse(x.InnerText)).OrderBy(x => x);

                var round = await kenoService.AddNewRoundResultAsync(
                    new CreateProVietKenoRound
                    {
                        Time = currentTime,
                        Number = latestRoundId,
                        Result = result
                    });

                await kenoService.AddRoundLogEntity(round.Number, round.Time, configuration.WebUrl);

                _ = Task.Run(() => kenoService.NotifyRoundResult(new[] { round }));
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