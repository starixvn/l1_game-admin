using System.Collections.Generic;
using GamesAdmin.Core.Extensions;

namespace GamesAdmin.Core.Enumeration
{
    public class SlotGame
    {
        private static readonly List<SlotGame> Games = new List<SlotGame>
        {
            new SlotGame((byte)GameId.FourGuardians,"Four Guardians"),
            new SlotGame((byte)GameId.NightClub,"Night Club") ,
            new SlotGame((byte)GameId.WayToGoal,"Way To Goal"),
            new SlotGame((byte)GameId.FortuneCat,"Fortune Cat" ),
            new SlotGame((byte)GameId.HeroesSlot,"Heroes Slot" )
        };

        public SlotGame(int id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
            ServiceName = SlotConfigExtensions.GetServiceName(displayName);
            Value = SlotConfigExtensions.GetGameName(displayName);
        }

        public int Id { get; }

        public string DisplayName { get; }

        public string Value { get; }

        public string ServiceName { get; }

        public static SlotGame FromGameId(byte id) => Games.Find(game => game.Id == id);

        public static SlotGame FromGameName(string gameName) => Games.Find(game => game.Value == gameName);

        public static bool IsSlotGame(string gameName) => Games.Exists(game => game.Value == gameName);
    }
}