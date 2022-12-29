using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site._Shared.Services.GeneralGameSettings
{
    public interface IGeneralGameSettingApi : IBaseAuthorizationApi
    {
        [Get("/general_game_settings/getAll")]
        Task<IEnumerable<GeneralGameSetting>> GetAll();
    }

}
