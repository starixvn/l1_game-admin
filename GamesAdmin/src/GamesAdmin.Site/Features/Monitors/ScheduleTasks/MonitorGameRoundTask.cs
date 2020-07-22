using Coravel.Invocable;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared;
using GamesAdmin.Site.Features._Shared;
using Sentry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
            var info = await service.GetGameRoundInfo();

            if (info?.Count == 0) 
            {
                sentryClient.CaptureMessage("Not found any game round info!!!", Sentry.Protocol.SentryLevel.Error);
            }

            var latestInfo = cache.GetCache<Dictionary<string, GameRoundInfo>>(CachedKeys.GameRoundInfo.ToString());

            if (latestInfo != null)
            {
                foreach (var latestGameInfo in latestInfo)
                {
                    var latestRound = latestGameInfo.Value;
                    var currentRound = info.ContainsKey(latestGameInfo.Key) ? info[latestGameInfo.Key] : null;

                    var notChanged = !currentRound.IsDisabled &&
                        latestRound.RoundId == currentRound.RoundId &&
                        latestRound.RemainingTime == currentRound.RemainingTime;

                    if (currentRound == null || notChanged)
                    {
                        alerts.Add($"Game {latestGameInfo.Key} stop at round {latestRound.RoundId} remainingTime {latestRound.RemainingTime}");
                    }
                }

                foreach (var alert in alerts)
                {
                    sentryClient.CaptureMessage(alert, Sentry.Protocol.SentryLevel.Error);
                }
            }

            cache.SetCache(CachedKeys.GameRoundInfo.ToString(), info);
        }
    }
}
