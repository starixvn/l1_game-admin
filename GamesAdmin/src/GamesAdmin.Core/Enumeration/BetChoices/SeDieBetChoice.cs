namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class SeDieBetChoice : Enumeration
    {
        public const string EvenValue = "even";
        public const string OddValue = "odd";
        public const string Red3Value = "red3";
        public const string White3Value = "white3";
        public const string Red4Value = "red4";
        public const string White4Value = "white4";

        public static readonly SeDieBetChoice Even = new SeDieBetChoice(EvenValue, "Even");
        public static readonly SeDieBetChoice Odd = new SeDieBetChoice(OddValue, "Odd");
        public static readonly SeDieBetChoice Red3 = new SeDieBetChoice(Red3Value, "Red3");
        public static readonly SeDieBetChoice White3 = new SeDieBetChoice(White3Value, "White3");
        public static readonly SeDieBetChoice Red4 = new SeDieBetChoice(Red4Value, "Red4");
        public static readonly SeDieBetChoice White4 = new SeDieBetChoice(White4Value, "White4");

        public SeDieBetChoice() { }

        public SeDieBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
