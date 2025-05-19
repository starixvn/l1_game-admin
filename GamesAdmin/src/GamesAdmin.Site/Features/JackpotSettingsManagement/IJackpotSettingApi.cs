using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement
{
    public interface IJackpotSettingApi : IBaseAuthorizationApi
    {
        [Post("/jackpot-settings/update-bot-ratio-settings")]
        Task<UpdateJackpotSettingsResult> UpdateBotRatioSettings(JackpotSettings settings);

        [Post("/jackpot-settings/update-bot-settings")]
        Task<UpdateJackpotSettingsResult> UpdateBotSettings(JackpotSettings settings);

        [Post("/jackpot-settings/update-player-settings")]
        Task<UpdateJackpotSettingsResult> UpdatePlayerSettings(JackpotSettings settings);

        [Get("/jackpot-settings/getall")]
        Task<IEnumerable<JackpotSettings>> GetAllJackpotSettings();

        [Get("/jackpot-settings/get/{configId}")]
        Task<JackpotSettings> GetJackpotSettings(string configId);
    }
}