using System.Collections.Generic;
using L1.games.admin.core.Models;

namespace L1.games.admin.site.Features.GameSettings.ViewModels
{
    public class GamesViewModel
    {
        public GamesViewModel(List<Game> games) 
        {
            GameList = games;
        }

        public List<Game> GameList { get;}
    }
}
