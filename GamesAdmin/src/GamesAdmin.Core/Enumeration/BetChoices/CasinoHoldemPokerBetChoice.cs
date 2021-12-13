namespace GamesAdmin.Core.Enumeration.BetChoices
{
    public class CasinoHoldemPokerBetChoice : Enumeration
    {
        public const string AnteBetValue = "anteBet";
        public const string AABetValue = "aaBet";
        public const string CallBetValue = "callBet";

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
