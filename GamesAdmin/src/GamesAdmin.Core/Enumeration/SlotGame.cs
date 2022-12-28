using System.Collections.Generic;

namespace GamesAdmin.Core.Enumeration
{
    public class SlotGame
    {
        private static readonly List<SlotGame> Games = new List<SlotGame>()
        {
            new SlotGame("Four Guardians", (byte)GameId.FourGuardians),
            new SlotGame("Night Club", (byte)GameId.NightClub),
            new SlotGame("Way To Goal", (byte)GameId.WayToGoal),
            new SlotGame("Fortune Cat", (byte)GameId.FortuneCat),
            new SlotGame("Heroes Slot", (byte)GameId.HeroesSlot)
        };

        public SlotGame()
        {
        }

        public SlotGame(string displayName, byte id)
        {
            Id = id;
            DisplayName = displayName;
            ServiceName = displayName.Replace(" ", "_").ToLower();
            Value = ServiceName.Replace("_", string.Empty);
        }

        public string DisplayName { get; }

        public string Value { get; }

        public byte Id { get; }

        public string ServiceName { get; }

        public static SlotGame FromGameId(byte id) => Games.Find(game => game.Id == id);

        public static SlotGame FromGameName(string gameName) => Games.Find(game => game.Value == gameName);

        public static bool IsSlotGame(string gameName) => Games.Exists(game => game.Value == gameName);

        public static IEnumerable<SlotGame> AllGames => Games;

        public static void AddSlotGame(SlotGame slotGame) => Games.Add(slotGame);
    }
}