using System;
using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models;
using Refit;
using Sentry;

namespace GamesAdmin.Api.GameServers
{
    public interface IGameServerService
    {
        Task UnderMaintenance(UMRequest request);

        Task Reload();

        Task<JsonWebToken> GetJWT(JWTRequest request);
    }

    public class GameServerService : IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISentryClient sentryClient;

        public GameServerService(
            IGameServerApi serverApi,
            ISentryClient sentryClient)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
        }

        public Task<JsonWebToken> GetJWT(JWTRequest request)
        {
            try
            {
                return serverApi.GetToken(request);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return Task.FromResult(new JsonWebToken());
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

        public async Task UnderMaintenance(UMRequest request)
        {
            try
            {
                var umRequest = new UnderMaintenanceRequest
                {
                    StartTime = request.StartTime.ToUniversalTime(),
                    EndTime = request.EndTime.ToUniversalTime(),
                    IsUM = request.IsUM
                };

                await serverApi.UnderMaintenance(umRequest);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
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
