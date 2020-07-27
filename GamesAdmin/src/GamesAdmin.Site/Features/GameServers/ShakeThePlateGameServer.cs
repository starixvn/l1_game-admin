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

        //[Get("/game/{name}/onlineusers")]
        //Task<OnlineUsers> GetOnlineUsersOnGame(string name);



        //[Get("/gameRoundInfo")]
        //Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo();
    }

    public interface IShakeThePlateGameServer
    {
        Task<OnlineUsers> GetOnlineUsers();
    }

    public class ShakeThePlateGameServer : IShakeThePlateGameServer
    {
        private readonly IShakeThePlateGameServerApi serverApi;
        private readonly ISentryClient sentryClient;

        public ShakeThePlateGameServer(IShakeThePlateGameServerApi serverApi, ISentryClient sentryClient)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
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

        private void HandleException(Exception ex, string title = null)
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
