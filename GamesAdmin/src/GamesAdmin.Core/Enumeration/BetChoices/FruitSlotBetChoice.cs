namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FruitSlotBetChoice : Enumeration
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

        public static readonly FruitSlotBetChoice Bar = new FruitSlotBetChoice(BarValue, "Bar");
        public static readonly FruitSlotBetChoice TripleSevent = new FruitSlotBetChoice(TripleSevenValue, "777");
        public static readonly FruitSlotBetChoice Star = new FruitSlotBetChoice(StarValue, "Star");
        public static readonly FruitSlotBetChoice Watermelon = new FruitSlotBetChoice(WatermeloValue, "Watermelon");
        public static readonly FruitSlotBetChoice Bell = new FruitSlotBetChoice(BellValue, "Bell");
        public static readonly FruitSlotBetChoice Lemon = new FruitSlotBetChoice(LemonValue, "Lemon");
        public static readonly FruitSlotBetChoice Orange = new FruitSlotBetChoice(OrangeValue, "Orange");
        public static readonly FruitSlotBetChoice Cherry = new FruitSlotBetChoice(CherryValue, "Cherry");
        public static readonly FruitSlotBetChoice Big = new FruitSlotBetChoice(BigValue, "Big");
        public static readonly FruitSlotBetChoice Small = new FruitSlotBetChoice(SmallValue, "Small");

        public FruitSlotBetChoice()
        { }

        public FruitSlotBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}