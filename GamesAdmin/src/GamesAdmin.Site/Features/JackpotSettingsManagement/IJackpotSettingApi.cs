using System.Threading.Tasks;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement
{
    public interface IJackpotSettingApi : IBaseAuthorizationApi
    {
        [Post("/jackpot-settings/update-bot-ratio-settings")]
        Task<UpdateJackpotSettingsResult> UpdateBotRatioSettings(Core.Models.GameConfigs.JackpotSettings settings);  

        [Post("/jackpot-settings/update-bot-settings")]
        Task<UpdateJackpotSettingsResult> UpdateBotSettings(Core.Models.GameConfigs.JackpotSettings settings);  

        [Post("/jackpot-settings/update-player-settings")]
        Task<UpdateJackpotSettingsResult> UpdatePlayerSettings(Core.Models.GameConfigs.JackpotSettings settings);  
    }
}