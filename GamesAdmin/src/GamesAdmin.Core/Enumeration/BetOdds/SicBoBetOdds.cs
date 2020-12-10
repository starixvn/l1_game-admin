namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class SicBoBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string AnyTripleValue = "anyTriple";

        public static readonly SicBoBetOdds Single = new SicBoBetOdds(SingleValue, "Single");
        public static readonly SicBoBetOdds Double = new SicBoBetOdds(DoubleValue, "Double");
        public static readonly SicBoBetOdds Triple = new SicBoBetOdds(TripleValue, "Triple");
        public static readonly SicBoBetOdds Big = new SicBoBetOdds(BigValue, "Big");
        public static readonly SicBoBetOdds Small = new SicBoBetOdds(SmallValue, "Small");
        public static readonly SicBoBetOdds AnyTriple = new SicBoBetOdds(AnyTripleValue, "Any Triple");
        public static readonly SicBoBetOdds TotalFour = new SicBoBetOdds("total4", "Total 4");
        public static readonly SicBoBetOdds TotalFive = new SicBoBetOdds("total5", "Total 5");
        public static readonly SicBoBetOdds TotalSix = new SicBoBetOdds("total6", "Total 6");
        public static readonly SicBoBetOdds TotalSeven = new SicBoBetOdds("total7", "Total 7");
        public static readonly SicBoBetOdds TotalEight = new SicBoBetOdds("total8", "Total 8");
        public static readonly SicBoBetOdds TotalNine = new SicBoBetOdds("total9", "Total 9");
        public static readonly SicBoBetOdds TotalTen = new SicBoBetOdds("total10", "Total 10");
        public static readonly SicBoBetOdds TotalEleven = new SicBoBetOdds("total11", "Total 11");
        public static readonly SicBoBetOdds TotalTwelve = new SicBoBetOdds("total12", "Total 12");
        public static readonly SicBoBetOdds TotalThirteen = new SicBoBetOdds("total13", "Total 13");
        public static readonly SicBoBetOdds TotalFourteen = new SicBoBetOdds("total14", "Total 14");
        public static readonly SicBoBetOdds TotalFifteen = new SicBoBetOdds("total15", "Total 15");
        public static readonly SicBoBetOdds TotalSixteen = new SicBoBetOdds("total16", "Total 16");
        public static readonly SicBoBetOdds TotalSeventeen = new SicBoBetOdds("total17", "Total 17");

        public SicBoBetOdds()
        {
        }

        public SicBoBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}