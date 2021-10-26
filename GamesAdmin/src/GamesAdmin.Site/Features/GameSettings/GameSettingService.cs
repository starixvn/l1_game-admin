using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.Chips;
using GamesAdmin.Site.Features.GameSettings.Requests;
using GamesAdmin.Site.Features.Partners;
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

        Task<OnlineUsers> GetOnlineUsers(string game = null);

        Task<bool> UpdateBotRatios(string name, double[] ratios);

        Task<bool> UpdateBetLimit(UpdateBetLimitRequest request);

        Task<bool> UpdateOdds(UpdateOddsRequest request);

        Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds);

        Task<bool> UpdateJackpot(UpdateJackpotRequest request);

        Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request);

        Task<bool> UpdateGeneralConfig(UpdateGeneralConfigRequest request);

        Task<bool> UpdateSiteExclusions(UpdateSiteExclusionRequest request);

        Task<GlobalConfig> GetSiteExclusion();

        Task<Dictionary<string, string>> GetDisabledMessage(string name);

        Task<GlobalConfig> GetSuperPlayers();

        Task<bool> UpdateSuperPlayers(UpdateSuperPlayersRequest request);

        Task<bool> UpdateBetaVersion(string[] names, bool isBeta);
    }

    public class GameSettingService : IGameSettingService
    {
        private readonly IGameSettingApi gameApi;
        private readonly IGlobalConfigApi globalConfigApi;
        private readonly ISentryClient sentryClient;

        public GameSettingService(ISentryClient sentryClient, IGameSettingApi gameApi, IGlobalConfigApi globalConfigApi)
        {
            this.sentryClient = sentryClient;
            this.gameApi = gameApi;
            this.globalConfigApi = globalConfigApi;
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

        public Task<OnlineUsers> GetOnlineUsers(string game = null)
        => gameApi.GetOnlineUsers(game);

        public Task<bool> UpdateBotRatios(string name, double[] ratios)
       => gameApi.UpdateBotRatios(name, ratios);

        public Task<bool> UpdateBetLimit(UpdateBetLimitRequest request)
        => gameApi.UpdateBetLimit(request);

        public Task<bool> UpdateOdds(UpdateOddsRequest request)
        => gameApi.UpdateOdds(request);

        public Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds)
        => gameApi.UpdateSlotOddss(name, iconOdds);

        public Task<bool> UpdateJackpot(UpdateJackpotRequest request)
        => gameApi.UpdateJackpot(request);

        public Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request)
        => gameApi.UpdateSlotConfig(request);

        public Task<bool> UpdateGeneralConfig(UpdateGeneralConfigRequest request)
        => gameApi.UpdateGeneralConfig(request);

        public Task<bool> UpdateSiteExclusions(UpdateSiteExclusionRequest request)
        {
            var formattedExlusions = string.IsNullOrWhiteSpace(request.SiteExclusions)
                ? new string[] { }
                : request.SiteExclusions.Trim().Split(";").Select(exclusion => exclusion.Trim()).ToArray();

            return globalConfigApi.UpdateSiteExclusions(request.ConfigName, formattedExlusions);
        }

        public Task<GlobalConfig> GetSiteExclusion()
            => globalConfigApi.GetSiteExclusions();

        public Task<Dictionary<string, string>> GetDisabledMessage(string name)
            => gameApi.GetDisableMessage(name);

        public Task<GlobalConfig> GetSuperPlayers() => globalConfigApi.GetSuperPlayers();

        public Task<bool> UpdateSuperPlayers(UpdateSuperPlayersRequest request)
        {
            var formattedNames = string.IsNullOrWhiteSpace(request.SuperPlayers)
                ? new string[] { }
                : request.SuperPlayers.Trim().Split(";").Select(p => p.Trim().ToLowerInvariant()).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

            return globalConfigApi.UpdateSuperPlayers(formattedNames);
        }

        public async Task<bool> UpdateBetaVersion(string[] names, bool enabled)
        {
            if (names?.Length == 0)
            {
                return false;
            }

            return await gameApi.UpdateBetaVersion(names, enabled);
        }
    }
}