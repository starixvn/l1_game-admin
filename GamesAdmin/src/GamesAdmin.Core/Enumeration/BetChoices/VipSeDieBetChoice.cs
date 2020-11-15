namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class VipSeDieBetChoice : Enumeration
    {
        public const string EvenValue = "even";
        public const string OddValue = "odd";
        public const string Red2Value = "red2";
        public const string Red3Value = "red3";
        public const string White3Value = "white3";
        public const string Red4Value = "red4";
        public const string White4Value = "white4";

        public static readonly VipSeDieBetChoice Even = new VipSeDieBetChoice(EvenValue, "Even");
        public static readonly VipSeDieBetChoice Odd = new VipSeDieBetChoice(OddValue, "Odd");
        public static readonly VipSeDieBetChoice Red2 = new VipSeDieBetChoice(Red2Value, "Red2");
        public static readonly VipSeDieBetChoice Red3 = new VipSeDieBetChoice(Red3Value, "Red3");
        public static readonly VipSeDieBetChoice White3 = new VipSeDieBetChoice(White3Value, "White3");
        public static readonly VipSeDieBetChoice Red4 = new VipSeDieBetChoice(Red4Value, "Red4");
        public static readonly VipSeDieBetChoice White4 = new VipSeDieBetChoice(White4Value, "White4");

        public VipSeDieBetChoice() { }

        public VipSeDieBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
