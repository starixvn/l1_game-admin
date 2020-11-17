namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipSicBoBetChoice : Enumeration
    {
        public const string OnePointValue = "1point";
        public const string TwoPointsValue = "2points";
        public const string ThreePointsValue = "3points";
        public const string FourPointsValue = "4points";
        public const string FivePointsValue = "5points";
        public const string SixPointsValue = "6points";
        public const string BigValue = "big";
        public const string SmallValue = "small";
        public const string OddValue = "odd";
        public const string EvenValue = "even";


        public static readonly VipSicBoBetChoice OnePoint = new VipSicBoBetChoice(OnePointValue, "1 point");
        public static readonly VipSicBoBetChoice TwoPoint = new VipSicBoBetChoice(TwoPointsValue, "2 points");
        public static readonly VipSicBoBetChoice ThreePoint = new VipSicBoBetChoice(ThreePointsValue, "3 points");
        public static readonly VipSicBoBetChoice FourPoint = new VipSicBoBetChoice(FourPointsValue, "4 points");
        public static readonly VipSicBoBetChoice FivePoint = new VipSicBoBetChoice(FivePointsValue, "5 points");
        public static readonly VipSicBoBetChoice SixPoint = new VipSicBoBetChoice(SixPointsValue, "6 points");
        public static readonly VipSicBoBetChoice Big = new VipSicBoBetChoice(BigValue, "Big");
        public static readonly VipSicBoBetChoice Small = new VipSicBoBetChoice(SmallValue, "Small");
        public static readonly VipSicBoBetChoice Odd = new VipSicBoBetChoice(OddValue, "Odd");
        public static readonly VipSicBoBetChoice Even = new VipSicBoBetChoice(EvenValue, "Even");
        public VipSicBoBetChoice() { }

        public VipSicBoBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
