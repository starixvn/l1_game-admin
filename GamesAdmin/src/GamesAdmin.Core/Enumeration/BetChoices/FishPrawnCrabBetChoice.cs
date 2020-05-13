namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FishPrawnCrabBetChoice : Enumeration
    {
        public const string FishValue = "fish";
        public const string PrawnValue = "prawn";
        public const string CrabValue = "crab";
        public const string StagValue = "stag";
        public const string GourdValue = "gourd";
        public const string RoosterValue = "rooster";

        public static readonly FishPrawnCrabBetChoice Fish = new FishPrawnCrabBetChoice(FishValue, "Fish");
        public static readonly FishPrawnCrabBetChoice Prawn = new FishPrawnCrabBetChoice(PrawnValue, "Prawn");
        public static readonly FishPrawnCrabBetChoice Crab = new FishPrawnCrabBetChoice(CrabValue, "Crab");
        public static readonly FishPrawnCrabBetChoice Stag = new FishPrawnCrabBetChoice(StagValue, "Stag");
        public static readonly FishPrawnCrabBetChoice Gourd = new FishPrawnCrabBetChoice(GourdValue, "Gourd");
        public static readonly FishPrawnCrabBetChoice Rooster = new FishPrawnCrabBetChoice(RoosterValue, "Rooster");

        public FishPrawnCrabBetChoice() { }

        public FishPrawnCrabBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
