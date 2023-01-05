using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features.GameSettings.Requests;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameSettingService
    {
        Task<IList<GameConfig>> GetAll();

        Task<bool> Add(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> IsExistName(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisabledMessage(UpdateDisabledMessageRequest request);

        Task<bool> Reload();

        Task<JsonWebToken> GetJWT(string username, string userAgent);

        Task<bool> UnderMaintenance(UnderMaintenanceRequest request);

        Task<OnlineUsers> GetOnlineUsers(int game = 0);

        Task<bool> UpdateBotRatios(string name, double[] ratios);

        Task<bool> UpdateBetLimit(UpdateBetLimitRequest request);

        Task<string> UpdateOdds(UpdateOddsRequest request);

        Task<string> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds);

        Task<string> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig);

        Task<bool> UpdateJackpot(UpdateJackpotRequest request);

        Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request);

        Task<bool> UpdateCascadingSlotsConfig(UpdateCascadingSlotsConfigRequest request);

        Task<bool> UpdateGeneralConfig(UpdateGeneralConfigRequest request);

        Task<bool> UpdateSlotRNGConfig(UpdateSlotRNGConfigRequest request);

        Task<Dictionary<string, string>> GetDisabledMessage(string name);

        Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username);

        Task<int> GetConnectedUsers();

        Task<bool> CloneNewSlotGame(int gameId, byte fromGameId, string displayName);
    }

    public class GameSettingService : IGameSettingService
    {
        private const string UpdateFailedMessage = "Update failed.";
        private readonly IGameSettingApi gameApi;
        private readonly IGlobalConfigApi globalConfigApi;
        private readonly ISentryClient sentryClient;
        private readonly ICascadingSlotsConfigValidator cascadingSlotsConfigValidator;
        private readonly IGeneralGameSettingService generalGameSettingService;

        public GameSettingService(
            ISentryClient sentryClient,
            IGameSettingApi gameApi,
            IGlobalConfigApi globalConfigApi,
            ICascadingSlotsConfigValidator cascadingSlotsConfigValidator,
            IGeneralGameSettingService generalGameSettingService)
        {
            this.sentryClient = sentryClient;
            this.gameApi = gameApi;
            this.globalConfigApi = globalConfigApi;
            this.cascadingSlotsConfigValidator = cascadingSlotsConfigValidator;
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<bool> Add(GameConfig game)
        => await gameApi.Add(game);

        public async Task<IList<GameConfig>> GetAll()
        {
            try
            {
                var gameConfigs = await gameApi.GetAll();

                return gameConfigs;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return new List<GameConfig>();
            }
        }

        public async Task<GameConfig> GetByName(string name)
            => await gameApi.GetByName(name);

        public async Task<bool> IsExistName(string name)
        {
            var gameConfig = await GetByName(name);

            return gameConfig != default;
        }

        public Task<bool> UpdateStatus(string[] names, bool enabled)
        => gameApi.UpdateStatus(names, enabled);

        public async Task<bool> UpdateDisabledMessage(UpdateDisabledMessageRequest request)
        {
            try
            {
                return await gameApi.UpdateMessage(request);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return false;
        }

        public Task<bool> Reload()
        => gameApi.Reload();

        public Task<JsonWebToken> GetJWT(string username, string userAgent)
        => gameApi.GetToken(new RequestJWT(username, userAgent));

        public Task<bool> UnderMaintenance(UnderMaintenanceRequest request)
        => gameApi.UnderMaintenance(request);

        public async Task<OnlineUsers> GetOnlineUsers(int game = 0)
        {
            var games = await generalGameSettingService.GetAllGames();

            var selectGame = game == 0 ? null : games.FirstOrDefault(gameInfo => gameInfo.GameId == game).Name;

            return await gameApi.GetOnlineUsers(selectGame);
        }

        public Task<bool> UpdateBotRatios(string name, double[] ratios)
       => gameApi.UpdateBotRatios(name, ratios);

        public Task<bool> UpdateBetLimit(UpdateBetLimitRequest request)
        => gameApi.UpdateBetLimit(request);

        public async Task<string> UpdateOdds(UpdateOddsRequest request)
        => await gameApi.UpdateOdds(request) ? string.Empty : UpdateFailedMessage;

        public async Task<string> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds)
        => await gameApi.UpdateSlotOdds(name, iconOdds) ? string.Empty : UpdateFailedMessage;

        public async Task<string> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig)
        {
            var validateResult = cascadingSlotsConfigValidator.ValidateOdds(cascadingSlotsConfig.OddsByCounts);
            if (!string.IsNullOrWhiteSpace(validateResult))
            {
                return validateResult;
            }

            return await gameApi.UpdateCascadingSlotOdds(name, cascadingSlotsConfig) ? string.Empty : UpdateFailedMessage;
        }

        public Task<bool> UpdateJackpot(UpdateJackpotRequest request)
        => gameApi.UpdateJackpot(request);

        public Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request)
        => gameApi.UpdateSlotConfig(request);

        public Task<bool> UpdateCascadingSlotsConfig(UpdateCascadingSlotsConfigRequest request)
        => gameApi.UpdateCascadingSlotsConfig(request);

        public Task<bool> UpdateGeneralConfig(UpdateGeneralConfigRequest request)
        => gameApi.UpdateGeneralConfig(request);

        public Task<bool> UpdateSlotRNGConfig(UpdateSlotRNGConfigRequest request)
        => gameApi.UpdateSlotRNGConfig(request);

        public Task<Dictionary<string, string>> GetDisabledMessage(string name)
            => gameApi.GetDisableMessage(name);

        public Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username)
            => gameApi.GetAdminMonitoringSettings(username);

        public Task<int> GetConnectedUsers()
            => gameApi.GetConnectedUsers();

        public Task<bool> CloneNewSlotGame(int gameId, byte fromGameId, string displayName)
            => gameApi.CloneNewSlotGame(gameId, fromGameId, displayName);
    }
}