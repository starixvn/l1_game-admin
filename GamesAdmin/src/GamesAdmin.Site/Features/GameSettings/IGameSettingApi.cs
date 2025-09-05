using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings.Requests;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameSettingApi : IBaseAuthorizationApi
    {
        [Get("/game_settings")]
        Task<IList<GameConfig>> GetAll();

        [Post("/game_settings/")]
        Task<GameCreationResult> Add([Body] GameConfig game);

        [Get("/game_settings/{name}")]
        Task<GameConfig> GetByName(string name);

        [Put("/game_settings/update_status")]
        Task<bool> UpdateStatus(string[] names, bool enabled);

        [Put("/game_settings/update_message")]
        Task<bool> UpdateMessage([Body] UpdateDisabledMessageRequest request);

        [Post("/game_server/reload")]
        Task<bool> Reload();

        [Post("/game_server/jwt")]
        Task<JsonWebToken> GetToken([Body] RequestJWT request);

        [Post("/um")]
        Task<bool> UnderMaintenance([Body] UnderMaintenanceRequest request);

        [Get("/game_server/online_users/{game}")]
        Task<OnlineUsers> GetOnlineUsers(string game = null);

        [Post("/game_settings/update_bot_ratios")]
        Task<bool> UpdateBotRatios([Body] UpdateBotRatioRequest request);

        [Post("/game_settings/update_bet_limit")]
        Task<bool> UpdateBetLimit([Body] UpdateBetLimitRequest request);

        [Post("/game_settings/update_odds")]
        Task<bool> UpdateOdds([Body] UpdateOddsRequest request);

        [Post("/game_settings/update_margin")]
        Task<bool> UpdateMarginSettings([Body] UpdateMarginSettingsRequest request);

        [Post("/game_settings/update_general_config")]
        Task<bool> UpdateGeneralConfig([Body] UpdateGeneralConfigRequest request);

        [Post("/game_settings/update_jackpot")]
        Task<bool> UpdateJackpot([Body] UpdateJackpotRequest request);

        [Post("/game_settings/update_slot_bot_config")]
        Task<bool> UpdateSlotBotConfig([Body] UpdateSlotBotConfigRequest request);

        [Post("/game_settings/update_slot_config")]
        Task<bool> UpdateSlotConfig([Body] UpdateSlotConfigRequest request);

        [Post("/game_settings/update_cascading_slots_config")]
        Task<bool> UpdateCascadingSlotsConfig([Body] UpdateCascadingSlotsConfigRequest request);

        [Post("/game_settings/update_slot_rng_config")]
        Task<bool> UpdateSlotRNGConfig([Body] UpdateSlotRNGConfigRequest request);

        [Post("/game_settings/update_crash_config")]
        Task<bool> UpdateCrashConfig([Body] UpdateCrashConfigRequest request);

        [Post("/game_settings/update_slot_odds")]
        Task<bool> UpdateSlotOdds([Body] UpdateSlotIconOddsRequest request);

        [Post("/game_settings/update_cascading_slot_odds")]
        Task<bool> UpdateCascadingSlotOdds([Body] UpdateOddsByCountsRequest request);

        [Get("/game_settings/{name}/disabled_message")]
        Task<Dictionary<string, string>> GetDisableMessage(string name);

        [Post("/game_settings/clone_slot_game")]
        Task<string> CloneSlotGame(int gameId, int fromGameId, string displayName, byte gameType);

        [Post("/game_settings/update_symbol_weight")]
        Task<bool> UpdateSymbolWeight([Body] UpdateSymbolWeightRequest request);

        [Post("/rtp_verify/getSlotRtp")]
        Task<RtpResult> GetSlotRtp([Body] GetSlotRtpRequest request);

        [Post("/game_server/get_admin_monitoring_settings")]
        Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username);

        [Get("/game_server/connected_users")]
        Task<int> GetConnectedUsers();

        [Post("/status_checker/check")]
        Task<StatusGameResult> CheckServiceStatus(CheckServiceStatusRequest request);
    }
}