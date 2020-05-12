namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class ShareThePlateBetChoice : Enumeration
    {
        public const string EvenValue = "even";
        public const string OddValue = "odd";
        public const string Red3Value = "red3";
        public const string White3Value = "white3";
        public const string Red4Value = "red4";
        public const string White4Value = "white4";

        public static readonly ShareThePlateBetChoice Even = new ShareThePlateBetChoice(EvenValue, "Even");
        public static readonly ShareThePlateBetChoice Odd = new ShareThePlateBetChoice(OddValue, "Odd");
        public static readonly ShareThePlateBetChoice Red3 = new ShareThePlateBetChoice(Red3Value, "Red3");
        public static readonly ShareThePlateBetChoice White3 = new ShareThePlateBetChoice(White3Value, "White3");
        public static readonly ShareThePlateBetChoice Red4 = new ShareThePlateBetChoice(Red4Value, "Red4");
        public static readonly ShareThePlateBetChoice White4 = new ShareThePlateBetChoice(White4Value, "White4");

        public ShareThePlateBetChoice() { }

        public ShareThePlateBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
