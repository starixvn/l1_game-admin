namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class VipSicBoBetOdds : Enumeration
    {
        public const string SingleValue = "single";
        public const string DoubleValue = "double";
        public const string TripleValue = "triple";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string AnyTripleValue = "anyTriple";

        public static readonly VipSicBoBetOdds Single = new VipSicBoBetOdds(SingleValue, "Single");
        public static readonly VipSicBoBetOdds Double = new VipSicBoBetOdds(DoubleValue, "Double");
        public static readonly VipSicBoBetOdds Triple = new VipSicBoBetOdds(TripleValue, "Triple");
        public static readonly VipSicBoBetOdds Big = new VipSicBoBetOdds(BigValue, "Big");
        public static readonly VipSicBoBetOdds Small = new VipSicBoBetOdds(SmallValue, "Small");
        public static readonly VipSicBoBetOdds AnyTriple = new VipSicBoBetOdds(AnyTripleValue, "Any Triple");
        public static readonly VipSicBoBetOdds FourPoints = new VipSicBoBetOdds("4", "4");
        public static readonly VipSicBoBetOdds FivePoints = new VipSicBoBetOdds("5", "5");
        public static readonly VipSicBoBetOdds SixPoints = new VipSicBoBetOdds("6", "6");
        public static readonly VipSicBoBetOdds SevenPoints = new VipSicBoBetOdds("7", "7");
        public static readonly VipSicBoBetOdds EightPoints = new VipSicBoBetOdds("8", "8");
        public static readonly VipSicBoBetOdds NinePoints = new VipSicBoBetOdds("9", "9");
        public static readonly VipSicBoBetOdds TenPoints = new VipSicBoBetOdds("10", "10");
        public static readonly VipSicBoBetOdds ElevenPoints = new VipSicBoBetOdds("11", "11");
        public static readonly VipSicBoBetOdds TwelvePoints = new VipSicBoBetOdds("12", "12");
        public static readonly VipSicBoBetOdds ThirteenPoints = new VipSicBoBetOdds("13", "13");
        public static readonly VipSicBoBetOdds FourteenPoints = new VipSicBoBetOdds("14", "14");
        public static readonly VipSicBoBetOdds FifteenPoints = new VipSicBoBetOdds("15", "15");
        public static readonly VipSicBoBetOdds SixteenPoints = new VipSicBoBetOdds("16", "16");
        public static readonly VipSicBoBetOdds SeventeenPoints = new VipSicBoBetOdds("17", "17");

        public VipSicBoBetOdds()
        {
        }

        public VipSicBoBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}