using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features._Shared.Components.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(List<GameType> games)
        {
            GameList = games;
        }

        public List<GameType> GameList { get; }
    }
}
