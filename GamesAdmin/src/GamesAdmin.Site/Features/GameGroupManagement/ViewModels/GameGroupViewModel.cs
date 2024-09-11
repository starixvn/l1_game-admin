using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameGroupManagement.ViewModels
{
    public class GameGroupViewModel
    {
        public string Name { get; set; }

        public IEnumerable<GameId> GameIds { get; set; }

        public string DisplayGames => string.Join(", ", GameIds.Select(x => Game.FromGameId(x).DisplayName));
    }
}
