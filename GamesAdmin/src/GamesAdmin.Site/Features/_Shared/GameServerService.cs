using System;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features._Shared
{
    public interface IGameServerApi : IBaseAuthorizationApi
    {
        [Post("/reload")]
        Task Reload();

        [Get("/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();

        [Get("/game/{name}/onlineusers")]
        Task<OnlineUsers> GetOnlineUsersOnGame(string name);
    }

    public interface IGameServerService
    {
        Task Reload();

        Task<OnlineUsers> GetOnlineUsers(string game);
    }

    public class GameServerService : IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISentryClient sentryClient;

        public GameServerService(IGameServerApi serverApi, ISentryClient sentryClient)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
        }

        public Task<OnlineUsers> GetOnlineUsers(string game)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(game))
                { 
                    return serverApi.GetOnlineUsers();
                }

                var gameType = string.Empty;
                switch (game)
                {
                    case GameType.ShakeThePlateValue:
                        gameType = "stp";
                        break;

                    case GameType.KenoProMaxValue:
                        gameType = "keno";
                        break;
                }

                return serverApi.GetOnlineUsersOnGame(gameType);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return Task.FromResult(new OnlineUsers());
        }

        public async Task Reload()
        {
            try
            {
                await serverApi.Reload();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex is ApiException)
            {
                var apiEx = ex as ApiException;

                if (apiEx != null)
                {
                    sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");
                }
            }

            sentryClient.CaptureException(ex);
        }

    }
}