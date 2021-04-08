namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FishPrawnCrabBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";

        public static readonly FishPrawnCrabBetOdds Single = new FishPrawnCrabBetOdds(SingleValue, "Single");
        public static readonly FishPrawnCrabBetOdds Double = new FishPrawnCrabBetOdds(DoubleValue, "Double");
        public static readonly FishPrawnCrabBetOdds Triple = new FishPrawnCrabBetOdds(TripleValue, "Triple");
        public FishPrawnCrabBetOdds() { }

        public FishPrawnCrabBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
