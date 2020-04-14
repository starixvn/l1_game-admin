using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site._Shared.Enumerations
{
    public class GameType : Enumeration
    {
        public static readonly GameType BigSmall = new GameType("taixiu", "Big Small");
        public static readonly GameType BigSmallTurbo = new GameType("txturbo", "Big Small Turbo");        
        public static readonly GameType FishPrawnCrab = new GameType("baucua", " Fish Prawn Crab");
        public static readonly GameType ShakeThePlate = new GameType("xocxoc", "Shake the Plate");
        public static readonly GameType OddEven = new GameType("oddeven", "Odd Even");
        public static readonly GameType OddEvenTurbo = new GameType("oeturbo", "Odd Even Turbo");

        public GameType()
        {
        }

        public GameType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
