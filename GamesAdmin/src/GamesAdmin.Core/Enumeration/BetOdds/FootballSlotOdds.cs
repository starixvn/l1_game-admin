namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FootballSlotOdds : Enumeration
    {
        public const string CupValue = "cup";
        public const string Cup50Value = "cup50";
        public const string Cup25Value = "cup25";
        public const string MascotValue = "mascot";
        public const string Mascot3Value = "mascot3";
        public const string FranceShirtValue = "franceShirt";
        public const string FranceShirt3Value = "franceShirt3";
        public const string HostCountryValue = "hostCountry";
        public const string HostCountry3Value = "hostCountry3";
        public const string EnglandShirtValue = "englandShirt";
        public const string EnglandShirt3Value = "englandShirt3";
        public const string TicketsValue = "tickets";
        public const string Tickets3Value = "tickets3";
        public const string BallValue = "ball";
        public const string Ball3Value = "ball3";
        public const string ShoesValue = "shoes";
        public const string Shoes3Value = "shoes3";
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly FootballSlotOdds Cup = new FootballSlotOdds(CupValue, "Cup");
        public static readonly FootballSlotOdds Cup50 = new FootballSlotOdds(Cup50Value, "Cup - x50");
        public static readonly FootballSlotOdds Cup25 = new FootballSlotOdds(Cup25Value, "Cup - x25");
        public static readonly FootballSlotOdds Mascot = new FootballSlotOdds(MascotValue, "Mascot");
        public static readonly FootballSlotOdds Mascot3 = new FootballSlotOdds(Mascot3Value, "Mascot - x3");
        public static readonly FootballSlotOdds HostCountry = new FootballSlotOdds(HostCountryValue, "HostCountry");
        public static readonly FootballSlotOdds HostCountry3 = new FootballSlotOdds(HostCountry3Value, "HostCountry - x3");
        public static readonly FootballSlotOdds FranceShirt = new FootballSlotOdds(FranceShirtValue, "FranceShirt");
        public static readonly FootballSlotOdds FranceShirt3 = new FootballSlotOdds(FranceShirt3Value, "FranceShirt - x3");
        public static readonly FootballSlotOdds EnglandShirt = new FootballSlotOdds(EnglandShirtValue, "EnglandShirt");
        public static readonly FootballSlotOdds EnglandShirt3 = new FootballSlotOdds(EnglandShirt3Value, "EnglandShirt - x3");
        public static readonly FootballSlotOdds Ball = new FootballSlotOdds(BallValue, "Ball");
        public static readonly FootballSlotOdds Ball3 = new FootballSlotOdds(Ball3Value, "Ball - x3");
        public static readonly FootballSlotOdds Tickets = new FootballSlotOdds(TicketsValue, "Tickets");
        public static readonly FootballSlotOdds Tickets3 = new FootballSlotOdds(Tickets3Value, "Tickets - x3");
        public static readonly FootballSlotOdds Shoes = new FootballSlotOdds(ShoesValue, "Shoes");
        public static readonly FootballSlotOdds Shoes3 = new FootballSlotOdds(Shoes3Value, "Shoes - x3");
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