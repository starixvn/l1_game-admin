namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class SicBoBetChoice : Enumeration
    {
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string AnyTripleValue = "anyTriple";

        public static readonly SicBoBetChoice Specific1 = new SicBoBetChoice("specific1", "Specific 1");
        public static readonly SicBoBetChoice Specific2 = new SicBoBetChoice("specific2", "Specific 2");
        public static readonly SicBoBetChoice Specific3 = new SicBoBetChoice("specific3", "Specific 3");
        public static readonly SicBoBetChoice Specific4 = new SicBoBetChoice("specific4", "Specific 4");
        public static readonly SicBoBetChoice Specific5 = new SicBoBetChoice("specific5", "Specific 5");
        public static readonly SicBoBetChoice Specific6 = new SicBoBetChoice("specific6", "Specific 6");
        public static readonly SicBoBetChoice Big = new SicBoBetChoice(BigValue, "Big");
        public static readonly SicBoBetChoice Small = new SicBoBetChoice(SmallValue, "Small");
        public static readonly SicBoBetChoice AnyTriple = new SicBoBetChoice(AnyTripleValue, "Any Triple");
        public static readonly SicBoBetChoice TotalFour = new SicBoBetChoice("total4", "Total 4");
        public static readonly SicBoBetChoice TotalFive = new SicBoBetChoice("total5", "Total 5");
        public static readonly SicBoBetChoice TotalSix = new SicBoBetChoice("total6", "Total 6");
        public static readonly SicBoBetChoice TotalSeven = new SicBoBetChoice("total7", "Total 7");
        public static readonly SicBoBetChoice TotalEight = new SicBoBetChoice("total8", "Total 8");
        public static readonly SicBoBetChoice TotalNine = new SicBoBetChoice("total9", "Total 9");
        public static readonly SicBoBetChoice TotalTen = new SicBoBetChoice("total10", "Total 10");
        public static readonly SicBoBetChoice TotalEleven = new SicBoBetChoice("total11", "Total 11");
        public static readonly SicBoBetChoice TotalTwelve = new SicBoBetChoice("total12", "Total 12");
        public static readonly SicBoBetChoice TotalThirteen = new SicBoBetChoice("total13", "Total 13");
        public static readonly SicBoBetChoice TotalFourteen = new SicBoBetChoice("total14", "Total 14");
        public static readonly SicBoBetChoice TotalFifteen = new SicBoBetChoice("total15", "Total 15");
        public static readonly SicBoBetChoice TotalSixteen = new SicBoBetChoice("total16", "Total 16");
        public static readonly SicBoBetChoice TotalSeventeen = new SicBoBetChoice("total17", "Total 17");
        public SicBoBetChoice() { }

        public SicBoBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
