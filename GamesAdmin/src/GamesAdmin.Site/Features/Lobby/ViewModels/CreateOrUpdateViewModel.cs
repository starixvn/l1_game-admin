using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class CreateOrUpdateViewModel
    {
        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }

        public LobbyModel LobbyConfig { get; set; }

        public List<SelectListItem> GameIds { get; set; }

        public IEnumerable<GeneralGameSetting> GameSettings { get; set; }

        public bool IsSuperAdmin { get; set; }

    }
}
