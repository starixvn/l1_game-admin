using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameServers;
using Polly;
using Polly.Retry;
using Refit;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

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

        [Post("/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);

        [Post("/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] RequestJWT request);

        [Get("/gameRoundInfo")]
        Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo();
    }

    public interface IGameServerService
    {
        Task Reload();

        Task<OnlineUsers> GetOnlineUsers(string game);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task<JsonWebToken> GetJWT(string username, string userAgent);

        Task<Dictionary<string, GameRoundInfo>> GetGameRoundInfo();
    }

    public class GameServerService : BaseGameServerService, IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly IShakeThePlateGameServer shakeThePlateGameServer;
        private readonly ISentryClient sentryClient;
        private AsyncRetryPolicy policy;

        public GameServerService(
            IGameServerApi serverApi,
            IShakeThePlateGameServer shakeThePlateGameServer,
            ISentryClient sentryClient)
            : base(sentryClient)
        {
            this.serverApi = serverApi;
            this.shakeThePlateGameServer = shakeThePlateGameServer;
            this.sentryClient = sentryClient;

            policy = Policy
             .Handle<HttpRequestException>()
             .Or<IOException>()
             .RetryAsync(3, onRetry: (exception, retryCount, context) =>
             {
                 sentryClient.CaptureMessage($"Retry {retryCount} , due to: {exception}.");
             });
        }

        public async Task<OnlineUsers> GetOnlineUsers(string game)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(game))
                {
                    return await serverApi.GetOnlineUsers();
                }

                var gameType = string.Empty;
                switch (game)
                {
                    case GameType.SeDieValue:
                        return await shakeThePlateGameServer.GetOnlineUsers();

                    case GameType.KenoProMaxValue:
                        gameType = "keno";
                        break;

                    case GameType.BaccaratValue:
                        gameType = "baccarat";
                        break;
                }

                return await serverApi.GetOnlineUsersOnGame(gameType);
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

                    await policy.ExecuteAsync(() => serverApi.NotifyRoundResult(round));
                }
                catch (Exception ex)
                {
                    HandleException(ex, $"{DateTime.Now} - NotifyRoundResult failed for round #{round.Number}.");
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
    }
}