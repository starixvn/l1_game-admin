namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipSicBoBetChoice : Enumeration
    {
        public const string SpecificNumberValue = "specificNumber";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string AnyTripleValue = "anyTriple";


        public static readonly VipSicBoBetChoice SpecificNumber = new VipSicBoBetChoice(SpecificNumberValue, "Specific Number");
        public static readonly VipSicBoBetChoice Big = new VipSicBoBetChoice(BigValue, "Big");
        public static readonly VipSicBoBetChoice Small = new VipSicBoBetChoice(SmallValue, "Small");
        public static readonly VipSicBoBetChoice AnyTriple = new VipSicBoBetChoice(AnyTripleValue, "Any Triple");
        public static readonly VipSicBoBetChoice FourPoints = new VipSicBoBetChoice("4", "4");
        public static readonly VipSicBoBetChoice FivePoints = new VipSicBoBetChoice("5", "5");
        public static readonly VipSicBoBetChoice SixPoints = new VipSicBoBetChoice("6", "6");
        public static readonly VipSicBoBetChoice SevenPoints = new VipSicBoBetChoice("7", "7");
        public static readonly VipSicBoBetChoice EightPoints = new VipSicBoBetChoice("8", "8");
        public static readonly VipSicBoBetChoice NinePoints = new VipSicBoBetChoice("9", "9");
        public static readonly VipSicBoBetChoice TenPoints = new VipSicBoBetChoice("10", "10");
        public static readonly VipSicBoBetChoice ElevenPoints = new VipSicBoBetChoice("11", "11");
        public static readonly VipSicBoBetChoice TwelvePoints = new VipSicBoBetChoice("12", "12");
        public static readonly VipSicBoBetChoice ThirteenPoints = new VipSicBoBetChoice("13", "13");
        public static readonly VipSicBoBetChoice FourteenPoints = new VipSicBoBetChoice("14", "14");
        public static readonly VipSicBoBetChoice FifteenPoints = new VipSicBoBetChoice("15", "15");
        public static readonly VipSicBoBetChoice SixteenPoints = new VipSicBoBetChoice("16", "16");
        public static readonly VipSicBoBetChoice SeventeenPoints = new VipSicBoBetChoice("17", "17");
        public VipSicBoBetChoice() { }

        public VipSicBoBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
