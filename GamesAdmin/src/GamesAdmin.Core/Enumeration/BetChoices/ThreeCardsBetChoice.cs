namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class ThreeCardsBetChoice : Enumeration
    {
        public const string RedValue = "red";
        public const string BlackValue = "black";
        public const string DiamondsValue = "diamonds";
        public const string HeartsValue = "hearts";
        public const string SpadesValue = "spades";
        public const string ClubsValue = "clubs";

        public static readonly ThreeCardsBetChoice Red = new ThreeCardsBetChoice(RedValue, "Red");
        public static readonly ThreeCardsBetChoice Black = new ThreeCardsBetChoice(BlackValue, "Black");
        public static readonly ThreeCardsBetChoice Diamonds = new ThreeCardsBetChoice(DiamondsValue, "Diamonds");
        public static readonly ThreeCardsBetChoice Hearts = new ThreeCardsBetChoice(HeartsValue, "Hearts");
        public static readonly ThreeCardsBetChoice Spades = new ThreeCardsBetChoice(SpadesValue, "Spades");
        public static readonly ThreeCardsBetChoice Clubs = new ThreeCardsBetChoice(ClubsValue, "Clubs");

        public ThreeCardsBetChoice() { }

        public ThreeCardsBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
