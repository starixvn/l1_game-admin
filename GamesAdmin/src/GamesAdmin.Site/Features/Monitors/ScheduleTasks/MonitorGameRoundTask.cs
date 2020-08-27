using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Monitors.Models;
using Sentry;

namespace GamesAdmin.Site.Features.Monitors.ScheduleTasks
{
    public class MonitorGameRoundTask : IInvocable
    {
        private readonly IGameServerService service;
        private readonly ISentryClient sentryClient;
        private readonly ICacheManager cache;

        public MonitorGameRoundTask(
            IGameServerService service,
            ISentryClient sentryClient,
            ICacheManager cache)
        {
            this.service = service;
            this.sentryClient = sentryClient;
            this.cache = cache;
        }

        public async Task Invoke()
        {
            var alerts = new List<string>();
            var currentGameRounds = await service.GetGameRoundInfo();

            if (currentGameRounds?.Count == 0)
            {
                sentryClient.CaptureMessage("Not found any game round info!!!", Sentry.Protocol.SentryLevel.Error);
            }

            var latestInfo = cache.GetCache<GameRoundInfoCache>(CachedKeys.GameRoundInfo.ToString());

            if (latestInfo?.GameRounds?.Count > 0)
            {
                foreach (var latestGameInfo in latestInfo.GameRounds)
                {
                    var currentRound = currentGameRounds.ContainsKey(latestGameInfo.Key) ? currentGameRounds[latestGameInfo.Key] : null;
                    if (currentRound == null)
                    {
                        alerts.Add($"{DateTime.Now} Game {latestGameInfo.Key} not found round info");
                        continue;
                    }

                    var latestRound = latestGameInfo.Value;

                    if (IsRoundUnchanged(currentRound, latestRound))
                    {
                        alerts.Add($"Game {latestGameInfo.Key} stop at round {latestRound.RoundId} remainingTime {latestRound.RemainingTime}. Latest info cached at {latestInfo.CachedDate}.");
                    }
                }

                foreach (var alert in alerts)
                {
                    sentryClient.CaptureMessage(alert, Sentry.Protocol.SentryLevel.Error);
                }
            }

            cache.SetCache(CachedKeys.GameRoundInfo.ToString(), new GameRoundInfoCache { CachedDate = DateTimeOffset.Now, GameRounds = currentGameRounds });
        }

        private static bool IsRoundUnchanged(GameRoundInfo currentRound, GameRoundInfo latestRound)
        => !currentRound.IsDisabled &&
                latestRound.RoundId == currentRound.RoundId &&
                latestRound.RemainingTime == currentRound.RemainingTime;
    }
}
