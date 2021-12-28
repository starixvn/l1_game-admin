using System.Threading.Tasks;
using Coravel.Invocable;
using GamesAdmin.Site._Shared;
using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.GameSettings;
using Sentry;
using Sentry.Protocol;

namespace GamesAdmin.Site.Features.Dashboard.ScheduleTasks
{
    public class MonitorOnlineUsers : IInvocable
    {
        private readonly IGameSettingService service;
        private readonly ISentryClient sentryClient;
        private readonly IAppSettings appSettings;
        private readonly ICacheManager cache;

        public MonitorOnlineUsers(
            IGameSettingService service,
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

            if (onlineUser.TotalRealPlayer < latestOnlineUsers - appSettings.MaxDecreased)
            {
                sentryClient.CaptureMessage(
                    $"Online users DOWN!!! Latest: {latestOnlineUsers} vs Current: {onlineUser.TotalRealPlayer}",
                    SentryLevel.Error);
            }

            cache.SetCache(CachedKeys.OnlineUsers.ToString(), onlineUser.TotalRealPlayer);
        }
    }
}