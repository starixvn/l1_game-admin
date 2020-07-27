using Refit;
using Sentry;
using System;

namespace GamesAdmin.Site.Features.GameServers
{
    public abstract class BaseGameServerService
    {
        private readonly ISentryClient sentryClient;

        public BaseGameServerService(ISentryClient sentryClient)
        {
            this.sentryClient = sentryClient;
        }

        protected void HandleException(Exception ex, string title = null)
        {
            if (ex is ApiException)
            {
                var apiEx = ex as ApiException;

                if (apiEx != null)
                {
                    sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                sentryClient.CaptureMessage(title, Sentry.Protocol.SentryLevel.Error);
            }

            sentryClient.CaptureException(ex);
        }
    }
}
