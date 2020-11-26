namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BlackjackBetChoice : Enumeration
    {
        public const string RegularValue = "regular";
        public const string BlackjackValue = "blackjack";

        public static readonly BlackjackBetChoice Regular = new BlackjackBetChoice(RegularValue, "Regular");
        public static readonly BlackjackBetChoice Blackjack = new BlackjackBetChoice(BlackjackValue, "Blackjack");

        public BlackjackBetChoice() { }

        public BlackjackBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
