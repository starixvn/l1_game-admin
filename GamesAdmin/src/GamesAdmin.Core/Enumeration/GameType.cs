namespace GamesAdmin.Core.Enumeration
{
    public class GameType : Enumeration
    {
        public const string SeDieValue = "xocxoc";
        public const string DragonTigerValue = "dragontiger";
        public const string BigSmallValue = "taixiu";
        public const string BigSmallTurboValue = "txturbo";
        public const string FishPrawnCrabValue = "baucua";
        public const string OddEvenValue = "oddeven";
        public const string OddEvenTurboValue = "oeturbo";
        public const string KenoProMaxValue = "kenopromax";
        public const string BaccaratValue = "baccarat";
        public const string MiniSeDieValue = "minisedie";

        public static readonly GameType BigSmall = new GameType(BigSmallValue, "Big Small");
        public static readonly GameType BigSmallTurbo = new GameType(BigSmallTurboValue, "Turbo Big Small");        
        public static readonly GameType FishPrawnCrab = new GameType(FishPrawnCrabValue, "Fish Prawn Crab");
        public static readonly GameType SeDie = new GameType(SeDieValue, "Se Die");
        public static readonly GameType OddEven = new GameType(OddEvenValue, "Odd Even");
        public static readonly GameType OddEvenTurbo = new GameType(OddEvenTurboValue, "Turbo Odd Even");
        public static readonly GameType DragonTiger = new GameType(DragonTigerValue, "Dragon Tiger");
        public static readonly GameType KenoProMax = new GameType(KenoProMaxValue, "Keno Pro Max");
        public static readonly GameType Baccarat = new GameType(BaccaratValue, "Baccarat");
        public static readonly GameType MiniSeDie = new GameType(MiniSeDieValue, "Mini Se Die");

        public GameType()
        {
        }

        public GameType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
