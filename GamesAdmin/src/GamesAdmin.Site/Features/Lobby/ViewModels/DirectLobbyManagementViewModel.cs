using GamesAdmin.Core.Models.Lobby;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Lobby.ViewModels
{
    public class DirectLobbyManagementViewModel
    {
        public IEnumerable<DirectLobbyModel> LobbyConfigs { get; set; }

        public IEnumerable<SelectListItem> CurrencyGroupItems { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool DisplayDeleteButton { get; set; } = true;
    }
}
