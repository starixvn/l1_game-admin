using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;
using Sentry;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameServers
{
    public interface IShakeThePlateGameServerApi : IBaseAuthorizationApi
    {
        [Get("/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();
    }

    public interface IShakeThePlateGameServer
    {
        Task<OnlineUsers> GetOnlineUsers();
    }

    public class ShakeThePlateGameServer : BaseGameServerService, IShakeThePlateGameServer
    {
        private readonly IShakeThePlateGameServerApi serverApi;

        public ShakeThePlateGameServer(IShakeThePlateGameServerApi serverApi, ISentryClient sentryClient) : base(sentryClient)
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
