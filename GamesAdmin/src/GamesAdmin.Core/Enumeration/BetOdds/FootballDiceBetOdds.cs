namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FootballDiceBetOdds : Enumeration
    {
        public const string HomeValue = "home";
        public const string AwayValue = "away";
        public const string DrawValue = "draw";

        public static readonly FootballDiceBetOdds Home = new FootballDiceBetOdds(HomeValue, "Home");
        public static readonly FootballDiceBetOdds Away = new FootballDiceBetOdds(AwayValue, "Away");
        public static readonly FootballDiceBetOdds Draw = new FootballDiceBetOdds(DrawValue, "Draw");

        public FootballDiceBetOdds()
        { }

        public FootballDiceBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}