using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameServers;
using Polly;
using Polly.Retry;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features._Shared
{
    public interface IGameServerApi : IBaseAuthorizationApi
    {
        [Post("/api/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);
    }

    public interface IGameServerService
    {
        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);
    }

    public class GameServerService : BaseGameServerService, IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISentryClient sentryClient;
        private AsyncRetryPolicy policy;

        public GameServerService(
            IGameServerApi serverApi,
            ISentryClient sentryClient)
            : base(sentryClient)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;

            policy = Policy
             .Handle<HttpRequestException>()
             .Or<IOException>()
             .RetryAsync(3, onRetry: (exception, retryCount, context) =>
             {
                 sentryClient.CaptureMessage($"Retry {retryCount} , due to: {exception}.");
             });
        }

        public async Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            foreach (var round in kenoRounds)
            {
                try
                {
                    sentryClient.CaptureMessage($"NotifyRoundResult {round.Number} at {DateTime.Now}");

                    await policy.ExecuteAsync(() => serverApi.NotifyRoundResult(round));
                }
                catch (Exception ex)
                {
                    HandleException(ex, $"{DateTime.Now} - NotifyRoundResult failed for round #{round.Number}.");
                }
            }
        }
    }
}