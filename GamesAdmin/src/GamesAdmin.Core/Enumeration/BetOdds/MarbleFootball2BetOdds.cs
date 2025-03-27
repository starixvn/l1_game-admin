namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class MarbleFootball2BetOdds: Enumeration
    {
        public const string FirstHalfHomeValue = "firstHalfHome";
        public const string FirstHalfAwayValue = "firstHalfAway";
        public const string FirstHalfDrawValue = "firstHalfDraw";
        public const string FirstHalfOverValue = "firstHalfOver";
        public const string FirstHalfUnderValue = "firstHalfUnder";
        public const string FirstHalfOddValue = "firstHalfOdd";
        public const string FirstHalfEvenValue = "firstHalfEven";

        public const string SecondHalfHomeValue = "secondHalfHome";
        public const string SecondHalfAwayValue = "secondHalfAway";
        public const string SecondHalfDrawValue = "secondHalfDraw";
        public const string SecondHalfOverValue = "secondHalfOver";
        public const string SecondHalfUnderValue = "secondHalfUnder";
        public const string SecondHalfOddValue = "secondHalfOdd";
        public const string SecondHalfEvenValue = "secondHalfEven";

        public static readonly MarbleFootball2BetOdds FirstHalfHome = new MarbleFootball2BetOdds(FirstHalfHomeValue, "Home");
        public static readonly MarbleFootball2BetOdds FirstHalfAway = new MarbleFootball2BetOdds(FirstHalfAwayValue, "Away");
        public static readonly MarbleFootball2BetOdds FirstHalfDraw = new MarbleFootball2BetOdds(FirstHalfDrawValue, "Draw");
        public static readonly MarbleFootball2BetOdds FirstHalfOver = new MarbleFootball2BetOdds(FirstHalfOverValue, "Over");
        public static readonly MarbleFootball2BetOdds FirstHalfUnder = new MarbleFootball2BetOdds(FirstHalfUnderValue, "Under");
        public static readonly MarbleFootball2BetOdds FirstHalfOdd = new MarbleFootball2BetOdds(FirstHalfOddValue, "Odd");
        public static readonly MarbleFootball2BetOdds FirstHalfEven = new MarbleFootball2BetOdds(FirstHalfEvenValue, "Even");

        public static readonly MarbleFootball2BetOdds SecondHalfHome = new MarbleFootball2BetOdds(SecondHalfHomeValue, "Home (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfAway = new MarbleFootball2BetOdds(SecondHalfAwayValue, "Away (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfDraw = new MarbleFootball2BetOdds(SecondHalfDrawValue, "Draw (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfOver = new MarbleFootball2BetOdds(SecondHalfOverValue, "Over (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfUnder = new MarbleFootball2BetOdds(SecondHalfUnderValue, "Under (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfOdd = new MarbleFootball2BetOdds(SecondHalfOddValue, "Odd (Live)");
        public static readonly MarbleFootball2BetOdds SecondHalfEven = new MarbleFootball2BetOdds(SecondHalfEvenValue, "Even (Live)");

        public MarbleFootball2BetOdds() {}
        public MarbleFootball2BetOdds(string value, string displayName) : base(value, displayName) {}   
    
    }
}