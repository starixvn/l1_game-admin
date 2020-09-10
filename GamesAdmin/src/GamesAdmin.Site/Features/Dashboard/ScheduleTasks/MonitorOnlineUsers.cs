using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Site._Shared;
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
        private readonly ICacheManager cache;

        public MonitorOnlineUsers(
            IGameServerService service,
            ISentryClient sentryClient,
            IAppSettings appSettings,
            ICacheManager cache)
        {
            this.service = service;
            this.sentryClient = sentryClient;
            this.appSettings = appSettings;
            this.cache = cache;
        }

        public async Task Invoke()
        {
            var onlineUser = await service.GetOnlineUsers();
            var latestOnlineUsers = cache.GetCache(CachedKeys.OnlineUsers.ToString());

            if (onlineUser.TotalReal < latestOnlineUsers - appSettings.MaxDecreased)
            {
                sentryClient.CaptureMessage(
                    $"Online users DOWN!!! Latest: {latestOnlineUsers} vs Current: {onlineUser.TotalReal}",
                    SentryLevel.Error);
            }

            cache.SetCache(CachedKeys.OnlineUsers.ToString(), onlineUser.TotalReal);
        }
    }
}
