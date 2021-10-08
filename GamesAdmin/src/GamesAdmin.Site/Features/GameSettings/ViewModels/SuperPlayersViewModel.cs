using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class SuperPlayersViewModel
    {
        public SuperPlayersViewModel(IEnumerable<string> superPlayers)
        {
            SuperPlayers = string.Join("; ", superPlayers);
        }

        public string SuperPlayers { get; set; }
    }
}
