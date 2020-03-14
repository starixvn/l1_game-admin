using System.Collections.Generic;
using L1.games.admin.core.Models;

namespace L1.games.admin.site.Features.GameSettings
{
    public interface IGameService
    {
        IList<Game> GetGames();
    }

    public class GameService : IGameService
    {
        public IList<Game> GetGames()
        => new List<Game>
            {
                new Game(1, "Cannon", true),
                new Game(2, "Clan Castle", false)
            };
    }
}
