namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoBetChoice : Enumeration
    {
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string EvenValue = "even";
        public const string OddValue = "odd";
        public const string EarthValue = "earth";
        public const string FireValue = "fire";
        public const string GoldValue = "gold";
        public const string WaterValue = "water";
        public const string WoodValue = "wood";

        public static readonly KenoBetChoice Big = new KenoBetChoice(BigValue, "Big");
        public static readonly KenoBetChoice Small = new KenoBetChoice(SmallValue, "Small");
        public static readonly KenoBetChoice Even = new KenoBetChoice(EvenValue, "Even");
        public static readonly KenoBetChoice Odd = new KenoBetChoice(OddValue, "Odd");
        public static readonly KenoBetChoice Earth = new KenoBetChoice(EarthValue, "Earth");
        public static readonly KenoBetChoice Fire = new KenoBetChoice(FireValue, "Fire");
        public static readonly KenoBetChoice Gold = new KenoBetChoice(GoldValue, "Gold");
        public static readonly KenoBetChoice Water = new KenoBetChoice(WaterValue, "Water");
        public static readonly KenoBetChoice Wood = new KenoBetChoice(WoodValue, "Wood");

        public KenoBetChoice() { }
        public KenoBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
