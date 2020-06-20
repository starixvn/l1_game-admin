using System;
using System.Collections.Generic;
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

        [Post("/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);

        [Post("/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] RequestJWT request);
    }

    public interface IGameServerService
    {
        Task Reload();

        Task<OnlineUsers> GetOnlineUsers(string game);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds, bool disabled = true);

        Task<JsonWebToken> GetJWT(string username, string userAgent);
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
                        gameType = "stp";
                        break;

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

        public async Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds, bool disabled = true)
        {
            if (disabled)
            {
                return;
            }

            foreach (var round in kenoRounds)
            {
                try
                {
                    sentryClient.CaptureMessage($"NotifyRoundResult {round.Number} at {DateTime.Now}");
                    await serverApi.NotifyRoundResult(round);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
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