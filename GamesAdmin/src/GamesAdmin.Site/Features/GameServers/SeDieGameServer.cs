using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameServers
{
    public interface ISeDieGameServerApi : IBaseAuthorizationApi
    {
        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();
    }

    public interface ISeDieGameServer
    {
        Task<OnlineUsers> GetOnlineUsers();
    }

    public class SeDieGameServer : BaseGameServerService, ISeDieGameServer
    {
        private readonly ISeDieGameServerApi serverApi;

        public SeDieGameServer(ISeDieGameServerApi serverApi, ISentryClient sentryClient) : base(sentryClient)
        {
            this.serverApi = serverApi;
        }

        public async Task<OnlineUsers> GetOnlineUsers()
        {
            try
            {
                return await serverApi.GetOnlineUsers();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
        }
    }
}
