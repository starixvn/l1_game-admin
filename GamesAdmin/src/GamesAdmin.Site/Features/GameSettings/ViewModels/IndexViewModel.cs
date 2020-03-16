using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel(List<Game> games) 
        {
            GameList = games;
        }

        public List<Game> GameList { get;}
    }
}
