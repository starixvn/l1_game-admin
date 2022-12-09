using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.Partners;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<LobbyModel> LobbyConfigs { get; set; }
        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }
        public IEnumerable<SelectListItem> CurrencyGroupItems { get; set; }
        public IEnumerable<Partner> Partners { get; set; }

    }
}
