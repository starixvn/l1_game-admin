using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Enumerations;
using System.Collections.Generic;

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
