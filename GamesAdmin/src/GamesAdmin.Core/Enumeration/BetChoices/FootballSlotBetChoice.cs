namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FootballSlotBetChoice : Enumeration
    {
        public const string BarValue = "bar";
        public const string TripleSevenValue = "tripleSeven";
        public const string WatermeloValue = "watermelon";
        public const string StarValue = "star";
        public const string BellValue = "bell";
        public const string OrangeValue = "orange";
        public const string LemonValue = "lemon";
        public const string CherryValue = "cherry";
        public const string BigValue = "big";
        public const string SmallValue = "small";

        public static readonly FootballSlotBetChoice Bar = new FootballSlotBetChoice(BarValue, "Bar");
        public static readonly FootballSlotBetChoice TripleSevent = new FootballSlotBetChoice(TripleSevenValue, "777");
        public static readonly FootballSlotBetChoice Star = new FootballSlotBetChoice(StarValue, "Star");
        public static readonly FootballSlotBetChoice Watermelon = new FootballSlotBetChoice(WatermeloValue, "Watermelon");
        public static readonly FootballSlotBetChoice Bell = new FootballSlotBetChoice(BellValue, "Bell");
        public static readonly FootballSlotBetChoice Lemon = new FootballSlotBetChoice(LemonValue, "Lemon");
        public static readonly FootballSlotBetChoice Orange = new FootballSlotBetChoice(OrangeValue, "Orange");
        public static readonly FootballSlotBetChoice Cherry = new FootballSlotBetChoice(CherryValue, "Cherry");
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