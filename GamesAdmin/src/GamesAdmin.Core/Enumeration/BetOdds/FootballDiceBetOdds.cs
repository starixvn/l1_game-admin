namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class FootballDiceBetOdds : Enumeration
    {
        public const string HomeValue = "home";
        public const string AwayValue = "away";
        public const string DrawValue = "draw";
        public const string Draw12Value = "draw12";

        public static readonly FootballDiceBetOdds Home = new FootballDiceBetOdds(HomeValue, "Home");
        public static readonly FootballDiceBetOdds Away = new FootballDiceBetOdds(AwayValue, "Away");
        public static readonly FootballDiceBetOdds Draw = new FootballDiceBetOdds(DrawValue, "Draw");
        public static readonly FootballDiceBetOdds Draw12 = new FootballDiceBetOdds(Draw12Value, "Draw 12");

        public FootballDiceBetOdds()
        { }

        public FootballDiceBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}