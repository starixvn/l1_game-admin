using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Lobby;
using GamesAdmin.Core.Models.Partners;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class CreateOrUpdateDirectLobbyViewModel
    {
        public IEnumerable<Partner> Operators { get; set; }

        public IEnumerable<CurrencyGroupModel> CurrencyGroupModels { get; set; }

        public DirectLobbyModel LobbyConfig { get; set; }

        public IEnumerable<GeneralGameSetting> GameSettings { get; set; }

        public bool IsSuperAdmin { get; set; }

    }
}
