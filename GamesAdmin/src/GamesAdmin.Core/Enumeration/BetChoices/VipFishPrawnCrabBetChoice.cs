namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipFishPrawnCrabBetChoice : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string AnyTripleValue = "anytriple";


        public static readonly VipFishPrawnCrabBetChoice Single = new VipFishPrawnCrabBetChoice(SingleValue, "Single");
        public static readonly VipFishPrawnCrabBetChoice Double = new VipFishPrawnCrabBetChoice(DoubleValue, "Double");
        public static readonly VipFishPrawnCrabBetChoice Triple = new VipFishPrawnCrabBetChoice(TripleValue, "Triple");
        public static readonly VipFishPrawnCrabBetChoice AnyTriple = new VipFishPrawnCrabBetChoice(AnyTripleValue, "Any Triple");

        public VipFishPrawnCrabBetChoice() { }

        public VipFishPrawnCrabBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
