namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FootballSlotOdds : Enumeration
    {
        public const string BarValue = "bar";
        public const string Bar50Value = "bar50";
        public const string Bar25Value = "bar25";
        public const string TripleSevenValue = "tripleSeven";
        public const string TripleSeven3Value = "tripleSeven3";
        public const string WatermeloValue = "watermelon";
        public const string Watermelo3Value = "watermelon3";
        public const string StarValue = "star";
        public const string Star3Value = "star3";
        public const string BellValue = "bell";
        public const string Bell3Value = "bell3";
        public const string OrangeValue = "orange";
        public const string Orange3Value = "orange3";
        public const string LemonValue = "lemon";
        public const string Lemon3Value = "lemon3";
        public const string CherryValue = "cherry";
        public const string Cherry3Value = "cherry3";
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly FootballSlotOdds Bar = new FootballSlotOdds(BarValue, "Bar");
        public static readonly FootballSlotOdds Bar50 = new FootballSlotOdds(Bar50Value, "Bar - x50");
        public static readonly FootballSlotOdds Bar25 = new FootballSlotOdds(Bar25Value, "Bar - x25");
        public static readonly FootballSlotOdds TripleSeven = new FootballSlotOdds(TripleSevenValue, "777");
        public static readonly FootballSlotOdds TripleSeven3 = new FootballSlotOdds(TripleSeven3Value, "777 - x3");
        public static readonly FootballSlotOdds Star = new FootballSlotOdds(StarValue, "Star");
        public static readonly FootballSlotOdds Star3 = new FootballSlotOdds(Star3Value, "Star - x3");
        public static readonly FootballSlotOdds Watermelo = new FootballSlotOdds(WatermeloValue, "Watermelon");
        public static readonly FootballSlotOdds Watermelo3 = new FootballSlotOdds(Watermelo3Value, "Watermelon - x3");
        public static readonly FootballSlotOdds Bell = new FootballSlotOdds(BellValue, "Bell");
        public static readonly FootballSlotOdds Bell3 = new FootballSlotOdds(Bell3Value, "Bell - x3");
        public static readonly FootballSlotOdds Lemon = new FootballSlotOdds(LemonValue, "Lemon");
        public static readonly FootballSlotOdds Lemon3 = new FootballSlotOdds(Lemon3Value, "Lemon - x3");
        public static readonly FootballSlotOdds Orange = new FootballSlotOdds(OrangeValue, "Orange");
        public static readonly FootballSlotOdds Orange3 = new FootballSlotOdds(Orange3Value, "Orange - x3");
        public static readonly FootballSlotOdds Cherry = new FootballSlotOdds(CherryValue, "Cherry");
        public static readonly FootballSlotOdds Cherry3 = new FootballSlotOdds(Cherry3Value, "Cherry - x3");
        public static readonly FootballSlotOdds Big = new FootballSlotOdds(BigValue, "Big");
        public static readonly FootballSlotOdds Small = new FootballSlotOdds(SmallValue, "Small");

        public FootballSlotOdds()
        { }

        public FootballSlotOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}