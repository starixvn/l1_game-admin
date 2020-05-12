using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Enumeration
{
    public class GameType : Enumeration
    {
        public const string ShakeThePlateValue = "xocxoc";
        public const string DragonTigerValue = "dragontiger";
        public const string BigSmallValue = "taixiu";
        public const string BigSmallTurboValue = "txturbo";
        public const string FishPrawnCrabValue = "baucua";
        public const string OddEvenValue = "oddeven";
        public const string OddEvenTurboValue = "oeturbo";
        public const string KenoProMaxValue = "kenopromax";
        public const string BaccaratValue = "baccarat";

        public static readonly GameType BigSmall = new GameType(BigSmallValue, "Big Small");
        public static readonly GameType BigSmallTurbo = new GameType(BigSmallTurboValue, "Big Small Turbo");        
        public static readonly GameType FishPrawnCrab = new GameType(FishPrawnCrabValue, "Fish Prawn Crab");
        public static readonly GameType ShakeThePlate = new GameType(ShakeThePlateValue, "Shake the Plate");
        public static readonly GameType OddEven = new GameType(OddEvenValue, "Odd Even");
        public static readonly GameType OddEvenTurbo = new GameType(OddEvenTurboValue, "Odd Even Turbo");
        public static readonly GameType DragonTiger = new GameType(DragonTigerValue, "Dragon Tiger");
        public static readonly GameType KenoProMax = new GameType(KenoProMaxValue, "Keno Pro Max");
        public static readonly GameType Baccarat = new GameType(BaccaratValue, "Baccarat");

        public GameType()
        {
        }

        public GameType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
