using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings.Requests;
using Refit;

namespace GamesAdmin.Site.Features.GameSettings
{
    public interface IGameSettingApi : IBaseAuthorizationApi
    {
        [Get("/game_settings")]
        Task<IList<GameConfig>> GetAll();

        [Post("/game_settings/")]
        Task<bool> Add([Body] GameConfig game);

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

        [Post("/game_settings/update_general_config")]
        Task<bool> UpdateGeneralConfig([Body] UpdateGeneralConfigRequest request);

        [Post("/game_settings/update_jackpot")]
        Task<bool> UpdateJackpot([Body] UpdateJackpotRequest request);

        [Post("/game_settings/update_slot_config")]
        Task<bool> UpdateSlotConfig([Body] UpdateSlotConfigRequest request);

        [Post("/game_settings/{name}/update_slot_odds")]
        Task<bool> UpdateSlotOddss(string name, [Body] Dictionary<string, Dictionary<string, double>> iconOdds);        
    }
}