namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class MarbleFootball2BetChoice : Enumeration
    {
        public const string FirstHalfHomeValue = "firstHalfHome";
        public const string SecondHalfHomeValue = "secondHalfHome";
        public const string FirstHalfAwayValue = "firstHalfAway";
        public const string SecondHalfAwayValue = "secondHalfAway";
        public const string FirstHalfDrawValue = "firstHalfDraw";
        public const string SecondHalfDrawValue = "secondHalfDraw";
        public const string FirstHalfOverValue = "firstHalfOver";
        public const string SecondHalfOverValue = "secondHalfOver";
        public const string FirstHalfUnderValue = "firstHalfUnder";
        public const string SecondHalfUnderValue = "secondHalfUnder";
        public const string FirstHalfOddValue = "firstHalfOdd";
        public const string SecondHalfOddValue = "secondHalfOdd";
        public const string FirstHalfEvenValue = "firstHalfEven";   
        public const string SecondHalfEvenValue = "secondHalfEven";

        public static readonly MarbleFootball2BetChoice FirstHalfHome = new MarbleFootball2BetChoice(FirstHalfHomeValue, "Home");
        public static readonly MarbleFootball2BetChoice SecondHalfHome = new MarbleFootball2BetChoice(SecondHalfHomeValue, "Home (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfAway = new MarbleFootball2BetChoice(FirstHalfAwayValue, "Away");
        public static readonly MarbleFootball2BetChoice SecondHalfAway = new MarbleFootball2BetChoice(SecondHalfAwayValue, "Away (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfDraw = new MarbleFootball2BetChoice(FirstHalfDrawValue, "Draw");
        public static readonly MarbleFootball2BetChoice SecondHalfDraw = new MarbleFootball2BetChoice(SecondHalfDrawValue, "Draw (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfOver = new MarbleFootball2BetChoice(FirstHalfOverValue, "Over");
        public static readonly MarbleFootball2BetChoice SecondHalfOver = new MarbleFootball2BetChoice(SecondHalfOverValue, "Over (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfUnder = new MarbleFootball2BetChoice(FirstHalfUnderValue, "Under");
        public static readonly MarbleFootball2BetChoice SecondHalfUnder = new MarbleFootball2BetChoice(SecondHalfUnderValue, "Under (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfOdd = new MarbleFootball2BetChoice(FirstHalfOddValue, "Odd");
        public static readonly MarbleFootball2BetChoice SecondHalfOdd = new MarbleFootball2BetChoice(SecondHalfOddValue, "Odd (Live)");
        public static readonly MarbleFootball2BetChoice FirstHalfEven = new MarbleFootball2BetChoice(FirstHalfEvenValue, "Even");
        public static readonly MarbleFootball2BetChoice SecondHalfEven = new MarbleFootball2BetChoice(SecondHalfEvenValue, "Even (Live)");

        public MarbleFootball2BetChoice() {}

        public MarbleFootball2BetChoice(string value, string displayName) : base(value, displayName)
        {
        }
    }
}
