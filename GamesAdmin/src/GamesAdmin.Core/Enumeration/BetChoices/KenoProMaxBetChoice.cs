namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoProMaxBetChoice : Enumeration
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

        public static readonly KenoProMaxBetChoice Big = new KenoProMaxBetChoice(BigValue, "Big");
        public static readonly KenoProMaxBetChoice Small = new KenoProMaxBetChoice(SmallValue, "Small");
        public static readonly KenoProMaxBetChoice Even = new KenoProMaxBetChoice(EvenValue, "Even");
        public static readonly KenoProMaxBetChoice Odd = new KenoProMaxBetChoice(OddValue, "Odd");
        public static readonly KenoProMaxBetChoice Earth = new KenoProMaxBetChoice(EarthValue, "Earth");
        public static readonly KenoProMaxBetChoice Fire = new KenoProMaxBetChoice(FireValue, "Fire");
        public static readonly KenoProMaxBetChoice Gold = new KenoProMaxBetChoice(GoldValue, "Gold");
        public static readonly KenoProMaxBetChoice Water = new KenoProMaxBetChoice(WaterValue, "Water");
        public static readonly KenoProMaxBetChoice Wood = new KenoProMaxBetChoice(WoodValue, "Wood");

        public KenoProMaxBetChoice() { }
        public KenoProMaxBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
