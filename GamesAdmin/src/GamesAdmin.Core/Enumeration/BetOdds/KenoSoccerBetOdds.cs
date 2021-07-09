namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class KenoSoccerBetOdds : Enumeration
    {
        public const string PreHTHomeValue = "preHTHome";
        public const string PreHTAwayValue = "preHTAway";
        public const string PreHTOverValue = "preHTOver";
        public const string PreHTUnderValue = "preHTUnder";
        public const string PreHTOddValue = "preHTOdd";
        public const string PreHTEvenValue = "preHTEven";

        public const string PreFTHomeValue = "preFTHome";
        public const string PreFTAwayValue = "preFTAway";
        public const string PreFTOverValue = "preFTOver";
        public const string PreFTUnderValue = "preFTUnder";
        public const string PreFTOddValue = "preFTOdd";
        public const string PreFTEvenValue = "preFTEven";

        public static readonly KenoSoccerBetOdds PreHTHome = new KenoSoccerBetOdds(PreHTHomeValue, "Pre-Match HT Home");
        public static readonly KenoSoccerBetOdds PreHTAway = new KenoSoccerBetOdds(PreHTAwayValue, "Pre-Match HT Away");
        public static readonly KenoSoccerBetOdds PreHTOver = new KenoSoccerBetOdds(PreHTOverValue, "Pre-Match HT Over");
        public static readonly KenoSoccerBetOdds PreHTUnder = new KenoSoccerBetOdds(PreHTUnderValue, "Pre-Match HT Under");
        public static readonly KenoSoccerBetOdds PreHTOdd = new KenoSoccerBetOdds(PreHTOddValue, "Pre-Match HT Odd");
        public static readonly KenoSoccerBetOdds PreHTEven = new KenoSoccerBetOdds(PreHTEvenValue, "Pre-Match HT Even");

        public static readonly KenoSoccerBetOdds PreFTHome = new KenoSoccerBetOdds(PreFTHomeValue, "Pre-Match FT Home");
        public static readonly KenoSoccerBetOdds PreFTAway = new KenoSoccerBetOdds(PreFTAwayValue, "Pre-Match FT Away");
        public static readonly KenoSoccerBetOdds PreFTOver = new KenoSoccerBetOdds(PreFTOverValue, "Pre-Match FT Over");
        public static readonly KenoSoccerBetOdds PreFTUnder = new KenoSoccerBetOdds(PreFTUnderValue, "Pre-Match FT Under");
        public static readonly KenoSoccerBetOdds PreFTOdd = new KenoSoccerBetOdds(PreFTOddValue, "Pre-Match FT Odd");
        public static readonly KenoSoccerBetOdds PreFTEven = new KenoSoccerBetOdds(PreFTEvenValue, "Pre-Match FT Even");

        public KenoSoccerBetOdds()
        {
        }

        public KenoSoccerBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}