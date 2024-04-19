namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FootballSlotBetChoice : Enumeration
    {
        public const string CupValue = "cup";
        public const string MascotValue = "mascot";
        public const string HostCountryValue = "hostCountry";
        public const string FranceShirtValue = "franceShirt";
        public const string EnglandShirtValue = "englandShirt";
        public const string BallValue = "ball";
        public const string TicketsValue = "tickets";
        public const string ShoesValue = "shoes";
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly FootballSlotBetChoice Cup = new FootballSlotBetChoice(CupValue, "Cup");
        public static readonly FootballSlotBetChoice Masscot = new FootballSlotBetChoice(MascotValue, "Mascot");
        public static readonly FootballSlotBetChoice HostCountry = new FootballSlotBetChoice(HostCountryValue, "Host Country");
        public static readonly FootballSlotBetChoice FranceShirt = new FootballSlotBetChoice(FranceShirtValue, "France Shirt");
        public static readonly FootballSlotBetChoice EnglandShirt = new FootballSlotBetChoice(EnglandShirtValue, "England Shirt");
        public static readonly FootballSlotBetChoice Ball = new FootballSlotBetChoice(BallValue, "Ball");
        public static readonly FootballSlotBetChoice Tickets = new FootballSlotBetChoice(TicketsValue, "Tickets");
        public static readonly FootballSlotBetChoice Shoes = new FootballSlotBetChoice(ShoesValue, "Shoes");
        public static readonly FootballSlotBetChoice Big = new FootballSlotBetChoice(BigValue, "Big");
        public static readonly FootballSlotBetChoice Small = new FootballSlotBetChoice(SmallValue, "Small");

        public FootballSlotBetChoice()
        { }

        public FootballSlotBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}