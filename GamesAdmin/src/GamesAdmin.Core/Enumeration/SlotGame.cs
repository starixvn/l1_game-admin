using System.Collections.Generic;

namespace GamesAdmin.Core.Enumeration
{
    public class SlotGame
    {
        private static readonly List<SlotGame> Games = new List<SlotGame>()
        {
            new SlotGame("Four Guardians", GameId.FourGuardians),
            new SlotGame("Night Club", GameId.NightClub),
            new SlotGame("Way To Goal", GameId.WayToGoal),
            new SlotGame("Fortune Cat", GameId.FortuneCat)
        };

        public SlotGame()
        {
        }

        public SlotGame(string displayName, GameId id)
        {
            GameId = id;
            DisplayName = displayName;
            ServiceName = displayName.Replace(" ", "_").ToLower();
            Value = ServiceName.Replace("_", string.Empty);
        }

        public string DisplayName { get; }

        public string Value { get; }

        public GameId GameId { get; }

        public string ServiceName { get; }

        public static SlotGame FromGameId(GameId id) => Games.Find(game => game.GameId == id);

        public static SlotGame FromGameName(string gameName) => Games.Find(game => game.Value == gameName);

        public static bool IsSlotGame(string gameName) => Games.Exists(game => game.Value == gameName);

        public static IEnumerable<SlotGame> AllGames => Games;
    }
}