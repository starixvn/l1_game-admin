using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features._Shared;
using Sentry;
using Sentry.Protocol;

namespace GamesAdmin.Site.Features.Dashboard.ScheduleTasks
{
    public class MonitorOnlineUsers : IInvocable
    {
        private readonly IGameServerService service;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;

        private int latestOnlineUsers = 0;

        public MonitorOnlineUsers(
            IGameServerService service, 
            ISentryClient sentryClient,
            IAppSettings appSettings)
        {
            this.service = service;
            this.sentryClient = sentryClient;
            this.appSettings = appSettings;
        }

        public async Task Invoke()
        {
            var onlineUser = await service.GetOnlineUsers(string.Empty);

            if (onlineUser.TotalReal < latestOnlineUsers - appSettings.MaxDecreased)
            {
                sentryClient.CaptureMessage(
                    $"Alert! Online users decreased dramatically. Latest: {latestOnlineUsers} vs Current: {onlineUser.TotalReal}",
                    SentryLevel.Error);
            }

            sentryClient.CaptureMessage($"Latest: {latestOnlineUsers} vs Current: {onlineUser.TotalReal}");

            latestOnlineUsers = onlineUser.TotalReal;
        }
    }
}
