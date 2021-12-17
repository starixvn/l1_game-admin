namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class TexasHoldemPokerBetChoice : Enumeration
    {
        public const string AnteBetValue = "anteBet";
        public const string CallBetValue = "callBet";
        public const string TurnBetValue = "turnBet";
        public const string RiverBetValue = "riverBet";

        public static readonly TexasHoldemPokerBetChoice AnteBet = new TexasHoldemPokerBetChoice(AnteBetValue, "Ante Bet");
        public static readonly TexasHoldemPokerBetChoice CallBet = new TexasHoldemPokerBetChoice(CallBetValue, "Call Bet");
        public static readonly TexasHoldemPokerBetChoice TurnBet = new TexasHoldemPokerBetChoice(TurnBetValue, "Turn Bet");
        public static readonly TexasHoldemPokerBetChoice RiverBet = new TexasHoldemPokerBetChoice(RiverBetValue, "River Bet");

        public TexasHoldemPokerBetChoice() { }

        public TexasHoldemPokerBetChoice(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
