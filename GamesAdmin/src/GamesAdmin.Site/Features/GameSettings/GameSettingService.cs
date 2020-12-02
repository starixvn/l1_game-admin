using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings.Requests;
using Refit;
using Sentry;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameSettingApi : IBaseAuthorizationApi
    {
        [Get("/game_settings")]
        Task<IList<GameConfig>> GetAll();

        [Post("/game_settings/")]
        Task<bool> Add([Body] GameConfig game);

        [Put("/game_settings/")]
        Task<bool> Update([Body] GameConfig game);

        [Get("/game_settings/{name}")]
        Task<GameConfig> GetByName(string name);

        [Put("/game_settings/update_status")]
        Task<bool> UpdateStatus(string[] names, bool enabled);

        [Put("/game_settings/{name}/update_message")]
        Task<bool> UpdateMessage(string name, string disabledMessage);

        [Put("/game_settings/update_message_all")]
        Task<bool> UpdateMessage(string disabledMessage);

        [Post("/game_server/reload")]
        Task<bool> Reload();

        [Post("/game_server/jwt")]
        Task<JsonWebToken> GetToken([Body] RequestJWT request);

        [Post("/um")]
        Task<bool> UnderMaintenance([Body] UnderMaintenanceRequest request);

        [Get("/game_server/online_users/{game}")]
        Task<OnlineUsers> GetOnlineUsers(string game = null);

        [Post("/game_settings/{name}/update_bot_ratios")]
        Task<bool> UpdateBotRatios(string name, double[] ratios);

        [Post("/game_settings/update_bet_limit_odds")]
        Task<bool> UpdateBetLimitAndOdds([Body] UpdateBetLimitAndOddsRequest request);

        //[Post("/game_settings/{name}/update_general_settings")]
        //Task<bool> UpdateGeneralSettings(string name, [Body] GameInfo gameInfo, [Body] BotSetting botSetting, [Body] EnableSetting enableSetting);

        //[Post("/game_settings/{name}/update_jackpot")]
        //Task<bool> UpdateJackpot(string name, [Body] Jackpot jackpot, [Body] List<StakeLevel> stakeLevels);

        //[Post("/game_settings/{name}/update_slot_settings")]
        //Task<bool> UpdateSlotSettings(string name, bool botEnabled, [Body] SlotsConfig slotsConfig);

        //[Post("/game_settings/{name}/update_slot_odds")]
        //Task<bool> UpdateSlotOddss(string name, [Body] Dictionary<string, Dictionary<string, float>> iconOdds);
    }

    public interface IGameSettingService
    {
        Task<IList<GameConfig>> GetAll();

        Task<bool> Update(GameConfig game);

        Task<bool> Add(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> IsExistName(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisabledMessage(string name, string message, bool isApplyAll = false);

        Task<bool> Reload();

        Task<JsonWebToken> GetJWT(string username, string userAgent);

        Task<bool> UnderMaintenance(UnderMaintenanceRequest request);

        Task<OnlineUsers> GetOnlineUsers(string game = null);

        Task<bool> UpdateBotRatios(string name, double[] ratios);

        Task<bool> UpdateBetLimitAndOdds(UpdateBetLimitAndOddsRequest request);

        //Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, float>> iconOdds);

        //Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels);

        //Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig);

        //Task<bool> UpdateGeneralSetting(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting);
    }

    public class GameSettingService : IGameSettingService
    {
        private readonly IGameSettingApi gameApi;
        private readonly ISentryClient sentryClient;

        public GameSettingService(ISentryClient sentryClient, IGameSettingApi gameApi)
        {
            this.sentryClient = sentryClient;
            this.gameApi = gameApi;
        }

        public async Task<bool> Add(GameConfig game)
        => await gameApi.Add(game);

        public async Task<IList<GameConfig>> GetAll()
        {
            try
            {
                return await gameApi.GetAll();
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return new List<GameConfig>();
            }
        }

        public async Task<bool> Update(GameConfig game)
        => await gameApi.Update(game);

        public async Task<GameConfig> GetByName(string name)
        => await gameApi.GetByName(name);

        public async Task<bool> IsExistName(string name)
        {
            var gameConfig = await GetByName(name);

            return gameConfig != default;
        }

        public Task<bool> UpdateStatus(string[] names, bool enabled)
        => gameApi.UpdateStatus(names, enabled);

        public async Task<bool> UpdateDisabledMessage(string name, string message, bool isApplyAll = false)
        {
            try
            {
                return isApplyAll ? await gameApi.UpdateMessage(message) : await gameApi.UpdateMessage(name, message);
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

        public Task<bool> UpdateBetLimitAndOdds(UpdateBetLimitAndOddsRequest request)
        => gameApi.UpdateBetLimitAndOdds(request);

        //public Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, float>> iconOdds)
        //=> gameApi.UpdateSlotOddss(name, iconOdds);

        //public Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels)
        //=> gameApi.UpdateJackpot(name, jackpot, stakeLevels);

        //public Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig)
        //=> gameApi.UpdateSlotSettings(name, botEnabled, slotsConfig);

        //public Task<bool> UpdateGeneralSetting(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
        //=> gameApi.UpdateGeneralSettings(name, gameInfo, botSetting, enableSetting);
    }
}