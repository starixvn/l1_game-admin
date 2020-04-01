using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site._Shared.Enumerations
{
    public class GameType : Enumeration
    {
        public static readonly GameType BigSmall = new GameType("taixiu", "Big Small");
        public static readonly GameType FishPrawnCrab = new GameType("baucua", " Fish Prawn Crab");
        public static readonly GameType ShakeThePlate = new GameType("xocxoc", "Shake the Plate");

        public GameType()
        {
        }

        public GameType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
