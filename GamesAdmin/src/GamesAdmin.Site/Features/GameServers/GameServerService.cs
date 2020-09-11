using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
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
        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetAllOnlineUsers();

        [Post("/api/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);     

        [Post("/api/um")]
        Task UnderMaintenance(UnderMaintenanceRequest request);
    }

    public interface IGameServerService
    {
        Task<OnlineUsers> GetOnlineUsers(GameType gameType = null, string game = null);

        Task NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task UnderMaintenance(UnderMaintenanceRequest request);
    }

    public class GameServerService : BaseGameServerService, IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly IOnlineUserApiFactory onlineUserFactory;
        private readonly ISentryClient sentryClient;
        private AsyncRetryPolicy policy;

        public GameServerService(
            IGameServerApi serverApi,
            ISentryClient sentryClient,
            IOnlineUserApiFactory onlineUserFactory)
            : base(sentryClient)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
            this.onlineUserFactory = onlineUserFactory;

            policy = Policy
             .Handle<HttpRequestException>()
             .Or<IOException>()
             .RetryAsync(3, onRetry: (exception, retryCount, context) =>
             {
                 sentryClient.CaptureMessage($"Retry {retryCount} , due to: {exception}.");
             });
        }

        public async Task<OnlineUsers> GetOnlineUsers(GameType gameType = null, string game = null)
        {
            try
            {
                if (gameType == null)
                {
                    return await serverApi.GetAllOnlineUsers();
                }

                var onlineUserService = onlineUserFactory.GetReportService(gameType);

                return await onlineUserService.GetOnlineUsers(game);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
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