namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FootballDiceBetOdds : Enumeration
    {
        public const string HomeValue = "high3";
        public const string AwayValue = "high4";
        public const string DrawValue = "high5";

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