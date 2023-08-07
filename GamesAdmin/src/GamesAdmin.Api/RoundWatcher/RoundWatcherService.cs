using GamesAdmin.Api.Lobby;
using GamesAdmin.Api.RoundWatcher;
using Refit;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Api
{
    public interface IRoundWatcherServiceApi
    {
        [Post("/api/lobby")]
        Task<RoundWatcherResult> NotifyToWatcher([Body] RoundWatcherRequest request);
    }

    public interface IRoundWatcherService
    {
        Task<RoundWatcherResult> NotifyToWatcher(RoundWatcherRequest request);
    }

    public class RoundWatcherService : IRoundWatcherService
    {
        private readonly ISentryClient sentryClient;
        private readonly IRoundWatcherServiceApi roundWatcherServiceApi;

        public RoundWatcherService(ISentryClient sentryClient, IRoundWatcherServiceApi roundWatcherServiceApi)
        {
            this.sentryClient = sentryClient;
            this.roundWatcherServiceApi = roundWatcherServiceApi;
        }

        public async Task<RoundWatcherResult> NotifyToWatcher(RoundWatcherRequest request)
        {
            try
            {
                var result = await roundWatcherServiceApi.NotifyToWatcher(request);
                return result;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return new RoundWatcherResult();
            }
        }
    }
}
