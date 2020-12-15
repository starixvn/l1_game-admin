namespace GamesAdmin.Core.Enumeration
{
    public class Game : Enumeration
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
        public const string FourGuardiansValue = "fourguardians";

        public static readonly Game BigSmall = new Game(BigSmallValue, "Big Small");
        public static readonly Game BigSmallTurbo = new Game(BigSmallTurboValue, "Turbo Big Small");        
        public static readonly Game FishPrawnCrab = new Game(FishPrawnCrabValue, "Fish Prawn Crab");
        public static readonly Game SeDie = new Game(SeDieValue, "Se Die");
        public static readonly Game OddEven = new Game(OddEvenValue, "Odd Even");
        public static readonly Game OddEvenTurbo = new Game(OddEvenTurboValue, "Turbo Odd Even");
        public static readonly Game DragonTiger = new Game(DragonTigerValue, "Dragon Tiger");
        public static readonly Game KenoProMax = new Game(KenoProMaxValue, "Keno Pro Max");
        public static readonly Game Baccarat = new Game(BaccaratValue, "Baccarat");
        public static readonly Game MiniSeDie = new Game(MiniSeDieValue, "Mini Se Die");
        public static readonly Game KenoProViet = new Game(KenoProVietValue, "Keno Pro Viet");
        public static readonly Game KenoMax = new Game(KenoMaxValue, "Keno Max");
        public static readonly Game KenoMax2 = new Game(KenoMax2Value, "Keno Max2");
        public static readonly Game KenoMini = new Game(KenoMiniValue, "Keno Mini");
        public static readonly Game KenoMini2 = new Game(KenoMini2Value, "Keno Mini 2");
        public static readonly Game KenoEast = new Game(KenoEastValue, "Keno East");
        public static readonly Game KenoWest = new Game(KenoWestValue, "Keno West");
        public static readonly Game KenoSouth = new Game(KenoSouthValue, "Keno South");
        public static readonly Game KenoNorth = new Game(KenoNorthValue, "Keno North");
        public static readonly Game Roulette = new Game(RouletteValue, "Roulette");
        public static readonly Game LuckyWheel = new Game(LuckyWheelValue, "Lucky Wheel");
        public static readonly Game VipSeDie = new Game(VipSeDieValue, "Vip Se Die");
        public static readonly Game SpecialSeDie = new Game(SpecialSeDieValue, "Special SeDie");
        public static readonly Game BlackJack = new Game(BlackJackValue, "Blackjack");
        public static readonly Game VipFishPrawnCrab = new Game(VipFishPrawnCrabValue, "Vip Fish Prawn Crab");
        public static readonly Game SicBo = new Game(SicBoValue, "Sic Bo");
        public static readonly Game FourGuadians = new Game(FourGuardiansValue, "Four Guardians");

        public Game()
        {
        }

        public Game(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
