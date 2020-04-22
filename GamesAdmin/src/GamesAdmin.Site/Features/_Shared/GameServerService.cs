using System;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features._Shared
{
    public interface IGameServerApi 
    {
        [Post("/reload")]
        Task Reload();

        [Get("/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();
    }

    public interface IGameServerService 
    {
        Task Reload();

        Task<int> GetOnlineUsers();
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

        public async Task<int> GetOnlineUsers()
        {
            try
            {
                var onlineUsers = await serverApi.GetOnlineUsers();

                return onlineUsers == null ? 0 : onlineUsers.TotalReal;
            }
            catch (Exception ex)
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

            return 0;
        }

        public async Task Reload()
        {
            try
            {
                await serverApi.Reload();
            }
            catch (Exception ex)
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
}
