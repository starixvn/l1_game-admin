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
        Task<bool> NotifyToWatcher([Body] RoundWatcherRequest request);
    }

    public interface IRoundWatcherService
    {
        Task<bool> NotifyToWatcher(RoundWatcherRequest request);
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

        public async Task<bool> NotifyToWatcher(RoundWatcherRequest request)
        {
            try
            {
                return await roundWatcherServiceApi.NotifyToWatcher(request);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }
            return false;
        }
    }
}
