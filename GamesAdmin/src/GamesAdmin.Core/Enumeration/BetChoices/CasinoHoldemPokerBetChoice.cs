namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class CasinoHoldemPokerBetChoice : Enumeration
    {
        public const string AnteBetValue = "AnteBet";
        public const string AABetValue = "AABet";
        public const string CallBetValue = "CallBet";

        public static readonly CasinoHoldemPokerBetChoice AnteBet = new CasinoHoldemPokerBetChoice(AnteBetValue, "AnteBet");
        public static readonly CasinoHoldemPokerBetChoice AABet = new CasinoHoldemPokerBetChoice(AABetValue, "AABet");
        public static readonly CasinoHoldemPokerBetChoice CallBet = new CasinoHoldemPokerBetChoice(CallBetValue, "CallBet");

        public CasinoHoldemPokerBetChoice() { }

        public CasinoHoldemPokerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
