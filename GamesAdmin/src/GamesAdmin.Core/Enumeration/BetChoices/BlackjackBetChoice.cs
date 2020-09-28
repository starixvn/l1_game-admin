namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class BlackjackBetChoice : Enumeration
    {
        public const string NormalValue = "normal";
        public const string BlackjackValue = "blackjack";

        public static readonly BlackjackBetChoice Normal = new BlackjackBetChoice(NormalValue, "Normal");
        public static readonly BlackjackBetChoice Blackjack = new BlackjackBetChoice(BlackjackValue, "Blackjack");

        public BlackjackBetChoice() { }

        public BlackjackBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
