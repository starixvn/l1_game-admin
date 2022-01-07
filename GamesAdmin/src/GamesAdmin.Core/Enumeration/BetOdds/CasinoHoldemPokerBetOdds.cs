namespace GamesAdmin.Core.Enumeration.BetOdds
{
    public class CasinoHoldemPokerBetOdds : Enumeration
    {
        //Call Bets
        public const string CallBetsValue = "CallBet_callBet";
        public static readonly CasinoHoldemPokerBetOdds CallBets = new CasinoHoldemPokerBetOdds(CallBetsValue, "Call Bet");

        // Ante Bets
        public const string AnteBetHightCardsValue = "AnteBet_highCards";
        public const string AnteBetOnePairValue = "AnteBet_onePair";
        public const string AnteBetTwoPairsValue = "AnteBet_twoPairs";
        public const string AnteBetThreeKindsValue = "AnteBet_threeKind";
        public const string AnteBetStraightValue = "AnteBet_straight";
        public const string AnteBetFlushValue = "AnteBet_flush";
        public const string AnteBetFullHouseValue = "AnteBet_fullHouse";
        public const string AnteBetFourKindsValue = "AnteBet_fourKind";
        public const string AnteBetStraightFlushValue = "AnteBet_straightFlush";
        public const string AnteBetRoyalFlushValue = "AnteBet_royalFlush";

        public static readonly CasinoHoldemPokerBetOdds AnteBetHightCards = new CasinoHoldemPokerBetOdds(AnteBetHightCardsValue, "Ante Bet High Cards");
        public static readonly CasinoHoldemPokerBetOdds AnteBetOnePair = new CasinoHoldemPokerBetOdds(AnteBetOnePairValue, "Ante Bet One Pair");
        public static readonly CasinoHoldemPokerBetOdds AnteBetTwoPairs = new CasinoHoldemPokerBetOdds(AnteBetTwoPairsValue, "Ante Bet Two Pairs");
        public static readonly CasinoHoldemPokerBetOdds AnteBetThreeKinds = new CasinoHoldemPokerBetOdds(AnteBetThreeKindsValue, "Ante Bet Three of a Kind");
        public static readonly CasinoHoldemPokerBetOdds AnteBetStraight = new CasinoHoldemPokerBetOdds(AnteBetStraightValue, "Ante Bet Straight");
        public static readonly CasinoHoldemPokerBetOdds AnteBetFlush = new CasinoHoldemPokerBetOdds(AnteBetFlushValue, "Ante Bet Flush");
        public static readonly CasinoHoldemPokerBetOdds AnteBetFullHouse = new CasinoHoldemPokerBetOdds(AnteBetFullHouseValue, "Ante Bet Full House");
        public static readonly CasinoHoldemPokerBetOdds AnteBetFourKinds = new CasinoHoldemPokerBetOdds(AnteBetFourKindsValue, "Ante Bet Four of a Kind");
        public static readonly CasinoHoldemPokerBetOdds AnteBetStraightFlush = new CasinoHoldemPokerBetOdds(AnteBetStraightFlushValue, "Ante Bet Straight Flush");
        public static readonly CasinoHoldemPokerBetOdds AnteBetRoyalFlush = new CasinoHoldemPokerBetOdds(AnteBetRoyalFlushValue, "Ante Bet Royal Flush");

        // AA Bets

        public const string AABetPairAcesValue = "AABet_pairAces";
        public const string AABetTwoPairsValue = "AABet_twoPairs";
        public const string AABetThreeKindsValue = "AABet_threeKind";
        public const string AABetStraightValue = "AABet_straight";
        public const string AABetFlushValue = "AABet_flush";
        public const string AABetFullHouseValue = "AABet_fullHouse";
        public const string AABetFourKindsValue = "AABet_fourKind";
        public const string AABetStraightFlushValue = "AABet_straightFlush";
        public const string AABetRoyalFlushValue = "AABet_royalFlush";

        public static readonly CasinoHoldemPokerBetOdds AABetPairAces = new CasinoHoldemPokerBetOdds(AABetPairAcesValue, "AA Bet Pair of Aces");
        public static readonly CasinoHoldemPokerBetOdds AABetTwoPairs = new CasinoHoldemPokerBetOdds(AABetTwoPairsValue, "AA Bet Two Pairs");
        public static readonly CasinoHoldemPokerBetOdds AABetThreeKinds = new CasinoHoldemPokerBetOdds(AABetThreeKindsValue, "AA Bet Three of a Kind");
        public static readonly CasinoHoldemPokerBetOdds AABetStraight = new CasinoHoldemPokerBetOdds(AABetStraightValue, "AA Bet Straight");
        public static readonly CasinoHoldemPokerBetOdds AABetFlush = new CasinoHoldemPokerBetOdds(AABetFlushValue, "AA Bet Flush");
        public static readonly CasinoHoldemPokerBetOdds AABetFullHouse = new CasinoHoldemPokerBetOdds(AABetFullHouseValue, "AA Bet Full House");
        public static readonly CasinoHoldemPokerBetOdds AABetFourKinds = new CasinoHoldemPokerBetOdds(AABetFourKindsValue, "AA Bet Four of a Kind");
        public static readonly CasinoHoldemPokerBetOdds AABetStraightFlush = new CasinoHoldemPokerBetOdds(AABetStraightFlushValue, "AA Bet Straight Flush");
        public static readonly CasinoHoldemPokerBetOdds AABetRoyalFlush = new CasinoHoldemPokerBetOdds(AABetRoyalFlushValue, "AA Bet Royal Flush");


        public CasinoHoldemPokerBetOdds()
        {
        }

        public CasinoHoldemPokerBetOdds(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}