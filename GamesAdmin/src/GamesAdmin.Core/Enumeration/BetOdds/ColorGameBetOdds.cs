namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class ColorGameBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string HotValue = "hot";
        public const string ColdValue = "cold";

        public static readonly ColorGameBetOdds Single = new ColorGameBetOdds(SingleValue, "Single");
        public static readonly ColorGameBetOdds Double = new ColorGameBetOdds(DoubleValue, "Double");
        public static readonly ColorGameBetOdds Triple = new ColorGameBetOdds(TripleValue, "Triple");
        public static readonly ColorGameBetOdds Hot = new ColorGameBetOdds(HotValue, "Hot");
        public static readonly ColorGameBetOdds Cold = new ColorGameBetOdds(ColdValue, "Cold");

        public ColorGameBetOdds() { }

        public ColorGameBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
