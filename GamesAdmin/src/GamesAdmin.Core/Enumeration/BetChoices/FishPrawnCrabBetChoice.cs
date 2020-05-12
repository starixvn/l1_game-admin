namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FishPrawnCrabBetChoice : Enumeration
    {
        public const string FishValue = "fish";
        public const string PrawnValue = "prawn";
        public const string CrabValue = "crab";

        public static readonly FishPrawnCrabBetChoice Fish = new FishPrawnCrabBetChoice(FishValue, "Fish");
        public static readonly FishPrawnCrabBetChoice Prawn = new FishPrawnCrabBetChoice(PrawnValue, "Prawn");
        public static readonly FishPrawnCrabBetChoice Crab = new FishPrawnCrabBetChoice(CrabValue, "Crab");

        public FishPrawnCrabBetChoice() { }

        public FishPrawnCrabBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
