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
        public const string KenoProVietValue = "kenoproviet";
        public const string KenoMaxValue = "kenomax";
        public const string KenoMax2Value = "kenomax2";
        public const string KenoMiniValue = "kenomini";
        public const string KenoMini2Value = "kenomini2";
        public const string KenoEastValue = "kenoeast";
        public const string KenoWestValue = "kenowest";
        public const string KenoSouthValue = "kenosouth";
        public const string KenoNorthValue = "kenonorth";
        public const string RouletteValue = "roulette";
        public const string LuckyWheelValue = "luckywheel";
        public const string VipSeDieValue = "vipsedie";
        public const string SpecialSeDieValue = "specialsedie";
        public const string BlackJackValue = "blackjack";
        public const string VipFishPrawnCrabValue = "vipfishprawncrab";
        public const string SicBoValue = "sicbo";

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
        public static readonly GameType KenoProViet = new GameType(KenoProVietValue, "Keno Pro Viet");
        public static readonly GameType KenoMax = new GameType(KenoMaxValue, "Keno Max");
        public static readonly GameType KenoMax2 = new GameType(KenoMax2Value, "Keno Max2");
        public static readonly GameType KenoMini = new GameType(KenoMiniValue, "Keno Mini");
        public static readonly GameType KenoMini2 = new GameType(KenoMini2Value, "Keno Mini 2");
        public static readonly GameType KenoEast = new GameType(KenoEastValue, "Keno East");
        public static readonly GameType KenoWest = new GameType(KenoWestValue, "Keno West");
        public static readonly GameType KenoSouth = new GameType(KenoSouthValue, "Keno South");
        public static readonly GameType KenoNorth = new GameType(KenoNorthValue, "Keno North");
        public static readonly GameType Roulette = new GameType(RouletteValue, "Roulette");
        public static readonly GameType LuckyWheel = new GameType(LuckyWheelValue, "Lucky Wheel");
        public static readonly GameType VipSeDie = new GameType(VipSeDieValue, "Vip Se Die");
        public static readonly GameType SpecialSeDie = new GameType(SpecialSeDieValue, "Special SeDie");
        public static readonly GameType BlackJack = new GameType(BlackJackValue, "Blackjack");
        public static readonly GameType VipFishPrawnCrab = new GameType(VipFishPrawnCrabValue, "Vip Fish Prawn Crab");
        public static readonly GameType SicBo = new GameType(SicBoValue, "Sic Bo");

        public GameType()
        {
        }

        public GameType(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
