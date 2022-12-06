using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.Partners;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class CreateOrUpdateViewModel
    {
        public IEnumerable<Partner> Partners { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }

        public LobbyModel LobbyConfig { get; set; }
    }
}
