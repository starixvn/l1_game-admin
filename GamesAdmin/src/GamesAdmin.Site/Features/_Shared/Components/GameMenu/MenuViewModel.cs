using GamesAdmin.Core.Models;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features._Shared.Components.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(List<GameConfig> games)
        {
            GameList = games;
        }

        public List<GameConfig> GameList { get; }
    }
}
