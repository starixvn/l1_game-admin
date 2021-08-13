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
        public const string VipBaccaratValue = "vipbaccarat";
        public const string VipSeDieWarValue = "vipsediewar";
        public const string FourGuardiansValue = "fourguardians";
        public const string LotteryValue = "lottery";
        public const string ThreeCardsValue = "threecards";
        public const string HeroesSlotValue = "heroesslot";
        public const string KenoMatch1Value = "kenomatch1";
        public const string KenoMatch2Value = "kenomatch2";

        public static readonly Game BigSmall = new Game(BigSmallValue, "Big Small", GameId.BigSmall);
        public static readonly Game BigSmallTurbo = new Game(BigSmallTurboValue, "Turbo Big Small", GameId.BigSmallTurbo);
        public static readonly Game FishPrawnCrab = new Game(FishPrawnCrabValue, "Fish Prawn Crab", GameId.FishPrawnCrab);
        public static readonly Game SeDie = new Game(SeDieValue, "Se Die", GameId.SeDie);
        public static readonly Game OddEven = new Game(OddEvenValue, "Odd Even", GameId.OddEven);
        public static readonly Game OddEvenTurbo = new Game(OddEvenTurboValue, "Turbo Odd Even", GameId.OddEvenTurbo);
        public static readonly Game DragonTiger = new Game(DragonTigerValue, "Dragon Tiger", GameId.DragonTiger);
        public static readonly Game KenoProMax = new Game(KenoProMaxValue, "Keno Pro Max", GameId.KenoProMax);
        public static readonly Game Baccarat = new Game(BaccaratValue, "Baccarat", GameId.Baccarat);
        public static readonly Game MiniSeDie = new Game(MiniSeDieValue, "Mini Se Die", GameId.MiniSeDie);
        public static readonly Game KenoProViet = new Game(KenoProVietValue, "Keno Pro Viet", GameId.KenoProViet);
        public static readonly Game KenoMax = new Game(KenoMaxValue, "Keno Max", GameId.KenoMax);
        public static readonly Game KenoMax2 = new Game(KenoMax2Value, "Keno Max2", GameId.KenoMax2);
        public static readonly Game KenoMini = new Game(KenoMiniValue, "Keno Mini", GameId.KenoMini);
        public static readonly Game KenoMini2 = new Game(KenoMini2Value, "Keno Mini 2", GameId.KenoMini2);
        public static readonly Game KenoEast = new Game(KenoEastValue, "Keno East", GameId.KenoEast);
        public static readonly Game KenoWest = new Game(KenoWestValue, "Keno West", GameId.KenoWest);
        public static readonly Game KenoSouth = new Game(KenoSouthValue, "Keno South", GameId.KenoSouth);
        public static readonly Game KenoNorth = new Game(KenoNorthValue, "Keno North", GameId.KenoNorth);
        public static readonly Game Roulette = new Game(RouletteValue, "Roulette", GameId.Roulette);
        public static readonly Game LuckyWheel = new Game(LuckyWheelValue, "Lucky Wheel");
        public static readonly Game VipSeDie = new Game(VipSeDieValue, "Vip Se Die", GameId.VipSeDie);
        public static readonly Game SpecialSeDie = new Game(SpecialSeDieValue, "Special SeDie", GameId.SpecialSeDie);
        public static readonly Game BlackJack = new Game(BlackJackValue, "Blackjack", GameId.Blackjack);
        public static readonly Game VipFishPrawnCrab = new Game(VipFishPrawnCrabValue, "Vip Fish Prawn Crab", GameId.VipFishPrawnCrab);
        public static readonly Game SicBo = new Game(SicBoValue, "Sic Bo", GameId.SicBo);
        public static readonly Game VipBaccarat = new Game(VipBaccaratValue, "Vip Baccarat", GameId.VipBaccarat);
        public static readonly Game VipSeDieWar = new Game(VipSeDieWarValue, "Se Die War", GameId.VipSeDieWar);
        public static readonly Game FourGuardians = new Game(FourGuardiansValue, "Four Guardians", GameId.FourGuardians);
        public static readonly Game Lottery = new Game(LotteryValue, "RNG Lottery", GameId.Lottery);
        public static readonly Game ThreeCards = new Game(ThreeCardsValue, "Three Cards", GameId.ThreeCards);
        public static readonly Game HeroesSlot = new Game(HeroesSlotValue, "Heroes Slot", GameId.HeroesSlot);
        public static readonly Game KenoMatch2 = new Game(KenoMatch2Value, "Keno Match 2", GameId.KenoMatch2);

        public Game()
        {
        }

        public Game(string value, string displayName)
            : base(value, displayName)
        {
        }

        public Game(string value, string displayName, GameId id)
            : base(value, displayName)
        {
            GameId = id;
        }

        public GameId GameId { get; }
    }
}
