namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class FootballDiceBetChoice : Enumeration
    {
        public const string HomeValue = "home";
        public const string AwayValue = "away";
        public const string DrawValue = "draw";

        public static readonly FootballDiceBetChoice Home = new FootballDiceBetChoice(HomeValue, "Home");
        public static readonly FootballDiceBetChoice Away = new FootballDiceBetChoice(AwayValue, "Away");
        public static readonly FootballDiceBetChoice Draw = new FootballDiceBetChoice(DrawValue, "Draw");

        public FootballDiceBetChoice() { }

        public FootballDiceBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
