using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Api.GameUser;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.SlotGame;
using Refit;
using Sentry;

namespace GamesAdmin.Api.GameServers
{
    public interface IGameServerService
    {
        Task UnderMaintenance(UMRequest request);

        Task Reload();

        Task<JsonWebToken> GetJWT(JWTRequest request);

        Task<OnlineUsers> GetOnlineUsers(Game gameType = null);

        Task<bool> NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds);

        Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request);
    }

    public class GameServerService : IGameServerService
    {
        private readonly IGameServerApi serverApi;
        private readonly ISentryClient sentryClient;
        private readonly IOnlineUserApiFactory onlineUserApiFactory;
        private readonly ISlotBoardApiFactory slotBoardApiFactory;
        private readonly IGameUserService gameUserService;
        private readonly IGameSettingsService gameSettingService;

        public GameServerService(
            IGameServerApi serverApi,
            ISentryClient sentryClient,
            IOnlineUserApiFactory onlineUserApiFactory,
            ISlotBoardApiFactory slotBoardApiFactory,
            IGameUserService gameUserService,
            IGameSettingsService gameSettingService)
        {
            this.serverApi = serverApi;
            this.sentryClient = sentryClient;
            this.onlineUserApiFactory = onlineUserApiFactory;
            this.slotBoardApiFactory = slotBoardApiFactory;
            this.gameUserService = gameUserService;
            this.gameSettingService = gameSettingService;
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

        public async Task<OnlineUsers> GetOnlineUsers(Game gameType = null)
        {
            try
            {
                if (gameType == null)
                {
                    return await serverApi.GetAllOnlineUsers();
                }

                var onlineUserService = onlineUserApiFactory.GetOnlineUserApi(gameType);

                return await onlineUserService.GetOnlineUsers(ConvertTypeToGameServerName(gameType));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return new OnlineUsers();
        }

        private static string ConvertTypeToGameServerName(Game gameType)
        {
            return gameType.Value switch
            {
                Game.KenoProMaxValue => "keno",
                Game.BaccaratValue => "baccarat",
                Game.RouletteValue => Game.RouletteValue,
                _ => string.Empty,
            };
        }

        private void HandleException(Exception ex, string title = null)
        {
            if (ex is ApiException apiEx)
            {
                sentryClient.CaptureMessage($"Request {apiEx.Uri} error: {apiEx.Message}");
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                sentryClient.CaptureMessage(title, Sentry.Protocol.SentryLevel.Error);
            }

            sentryClient.CaptureException(ex);
        }

        public async Task<bool> NotifyRoundResult(IEnumerable<ProVietKenoRound> kenoRounds)
        {
            foreach (var round in kenoRounds)
            {
                try
                {
                    sentryClient.CaptureMessage($"NotifyRoundResult {round.Number} at {DateTime.Now}");

                    await serverApi.NotifyRoundResult(round);
                }
                catch (Exception ex)
                {
                    HandleException(ex, $"{DateTime.Now} - NotifyRoundResult failed for round #{round.Number}.");
                }
            }

            return true;
        }

        public async Task<SettlementBoard> SetSlotBoard(SetSlotBoardRequest request)
        {
            try
            {
                var config = await gameSettingService.GetByName(request.GameConfigName);
                var isExcludeUUS = (config?.SlotsConfig?.Jackpot) != null && config.SlotsConfig.Jackpot.ExcludeUUS;

                if (await gameUserService.IsAbleToSetSlotResult(request.Nickname, isExcludeUUS))
                {
                    var gameEnum = Enumeration.FromValue<Game>(request.GameConfigName);

                    var result = await slotBoardApiFactory.SetBoardApi(gameEnum).SetBoard(new SetSlotBoardServerRequest(request.Nickname, request.Results));
                    result.Result = request.Results.ToList();

                    return result;
                }                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}