namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class VipFishPrawnCrabBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string AnyTripleValue = "anyTriple";
        public const string MountainValue = "mountain";
        public const string OceanValue = "ocean";

        public static readonly VipFishPrawnCrabBetOdds Single = new VipFishPrawnCrabBetOdds(SingleValue, "Single");
        public static readonly VipFishPrawnCrabBetOdds Double = new VipFishPrawnCrabBetOdds(DoubleValue, "Double");
        public static readonly VipFishPrawnCrabBetOdds Triple = new VipFishPrawnCrabBetOdds(TripleValue, "Triple");
        public static readonly VipFishPrawnCrabBetOdds AnyTriple = new VipFishPrawnCrabBetOdds(AnyTripleValue, "Any Triple");
        public static readonly VipFishPrawnCrabBetOdds Mountain = new VipFishPrawnCrabBetOdds(MountainValue, "Mountain");
        public static readonly VipFishPrawnCrabBetOdds Ocean = new VipFishPrawnCrabBetOdds(OceanValue, "Ocean");
        public VipFishPrawnCrabBetOdds() { }

        public VipFishPrawnCrabBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
