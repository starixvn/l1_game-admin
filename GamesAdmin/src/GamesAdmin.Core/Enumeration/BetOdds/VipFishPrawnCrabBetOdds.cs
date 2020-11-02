namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class VipFishPrawnCrabBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string AnyTripleValue = "anytriple";


        public static readonly VipFishPrawnCrabBetOdds Single = new VipFishPrawnCrabBetOdds(SingleValue, "Single");
        public static readonly VipFishPrawnCrabBetOdds Double = new VipFishPrawnCrabBetOdds(DoubleValue, "Double");
        public static readonly VipFishPrawnCrabBetOdds Triple = new VipFishPrawnCrabBetOdds(TripleValue, "Triple");
        public static readonly VipFishPrawnCrabBetOdds AnyTriple = new VipFishPrawnCrabBetOdds(AnyTripleValue, "Any Triple");

        public VipFishPrawnCrabBetOdds() { }

        public VipFishPrawnCrabBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
