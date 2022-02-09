namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class CasinoHoldemPokerBetChoice : Enumeration
    {
        public const string AnteBetValue = "anteBet";
        public const string AABetValue = "aaBet";
        public const string CallBetValue = "callBet";

        public static readonly CasinoHoldemPokerBetChoice AnteBet = new CasinoHoldemPokerBetChoice(AnteBetValue, "Ante");
        public static readonly CasinoHoldemPokerBetChoice AABet = new CasinoHoldemPokerBetChoice(AABetValue, "AA");
        public static readonly CasinoHoldemPokerBetChoice CallBet = new CasinoHoldemPokerBetChoice(CallBetValue, "Call");

        public CasinoHoldemPokerBetChoice() { }

        public CasinoHoldemPokerBetChoice(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}
