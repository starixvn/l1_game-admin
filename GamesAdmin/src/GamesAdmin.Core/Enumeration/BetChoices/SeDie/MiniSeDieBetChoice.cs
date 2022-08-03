namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class MiniSeDieBetChoice : Enumeration
    {
        public const string EvenValue = "even";
        public const string OddValue = "odd";
        public const string Red3Value = "red3";
        public const string White3Value = "white3";
        public const string Red4Value = "red4";
        public const string White4Value = "white4";

        public static readonly MiniSeDieBetChoice Even = new MiniSeDieBetChoice(EvenValue, "Even");
        public static readonly MiniSeDieBetChoice Odd = new MiniSeDieBetChoice(OddValue, "Odd");
        public static readonly MiniSeDieBetChoice Red3 = new MiniSeDieBetChoice(Red3Value, "Red3");
        public static readonly MiniSeDieBetChoice White3 = new MiniSeDieBetChoice(White3Value, "White3");
        public static readonly MiniSeDieBetChoice Red4 = new MiniSeDieBetChoice(Red4Value, "Red4");
        public static readonly MiniSeDieBetChoice White4 = new MiniSeDieBetChoice(White4Value, "White4");

        public MiniSeDieBetChoice() { }

        public MiniSeDieBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
