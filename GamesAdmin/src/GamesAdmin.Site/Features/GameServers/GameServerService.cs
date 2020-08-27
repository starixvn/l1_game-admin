using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameServers;
using GamesAdmin.Site.Features.GameSettings.Requests;
using Polly;
using Polly.Retry;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features._Shared
{
    public interface IGameServerApi : IBaseAuthorizationApi
    {
        [Post("/api/reload")]
        Task Reload();

        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetOnlineUsers();

        [Get("/api/game/{name}/onlineusers")]
        Task<OnlineUsers> GetOnlineUsersOnGame(string name);

        [Post("/api/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);

        [Post("/api/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] RequestJWT request);

        [Get("/api/gameRoundInfo")]
        Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo();

        [Post("/api/um")]
        Task UnderMaintenance(UnderMaintenanceRequest request);
    }

    public interface IGameServerService
    {
        Task Reload();

        Task<OnlineUsers> GetOnlineUsers(string game);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task<JsonWebToken> GetJWT(string username, string userAgent);

        Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo();

        Task UnderMaintenance(UnderMaintenanceRequest request);
    }

    public class GameServerService : BaseGameServerService, IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISeDieGameServer seDieGameServer;
        private readonly ISentryClient sentryClient;
        private AsyncRetryPolicy policy;

        public GameServerService(
            IGameServerApi serverApi,
            ISeDieGameServer seDieGameServer,
            ISentryClient sentryClient)
            : base(sentryClient)
        {
            this.serverApi = serverApi;
            this.seDieGameServer = seDieGameServer;
            this.sentryClient = sentryClient;

            policy = Policy
             .Handle<HttpRequestException>()
             .Or<IOException>()
             .RetryAsync(2, onRetry: (exception, retryCount, context) =>
             {
                 sentryClient.CaptureMessage($"Retry {retryCount} , due to: {exception}.");
             });
        }

        public async Task<OnlineUsers> GetOnlineUsers(string game)
        {
            try
            {
                return string.IsNullOrWhiteSpace(game)
                    ? await serverApi.GetOnlineUsers()
                    : await serverApi.GetOnlineUsersOnGame(game);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
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

        public Task<JsonWebToken> GetJWT(string username, string userAgent)
        {
            try
            {
                return serverApi.GetToken(new RequestJWT(username, userAgent));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return Task.FromResult(new JsonWebToken());
        }

        public async Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            foreach (var round in kenoRounds)
            {
                try
                {
                    sentryClient.CaptureMessage($"NotifyRoundResult {round.Number} at {DateTime.Now}");

                    await serverApi.NotifyRoundResult(round);
                    await policy.ExecuteAsync(() => serverApi.NotifyRoundResult(round));
                }
                catch (Exception ex)
                {
                    HandleException(ex, $"{DateTime.Now} - NotifyRoundResult failed for #{round.Number}.");
                }
            }
        }

        public async Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo()
        {
            try
            {
                return await serverApi.GetGameRoundInfo();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new Dictionary<string, GameRoundInfo>();
        }

        public async Task UnderMaintenance(UnderMaintenanceRequest request)
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
    }
}