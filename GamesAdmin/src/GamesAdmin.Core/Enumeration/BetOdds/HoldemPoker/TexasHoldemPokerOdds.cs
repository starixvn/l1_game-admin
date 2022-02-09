namespace GamesAdmin.Core.Enumeration.BetOdds
{
    class TexasHoldemPokerOdds : Enumeration
    {
        public const string CallValue = "call";
        public const string TurnValue = "turn";
        public const string RiverValue = "river";

        public static readonly TexasHoldemPokerOdds Call = new TexasHoldemPokerOdds(CallValue, "Call Bet");
        public static readonly TexasHoldemPokerOdds Turn = new TexasHoldemPokerOdds(TurnValue, "Turn Bet");
        public static readonly TexasHoldemPokerOdds River = new TexasHoldemPokerOdds(RiverValue, "River Bet");

        // Ante Bets
        public const string AnteBetHighCardValue = "AnteBet_highCard";
        public const string AnteBetOnePairValue = "AnteBet_onePair";
        public const string AnteBetTwoPairsValue = "AnteBet_twoPairs";
        public const string AnteBetThreeKindsValue = "AnteBet_threeKind";
        public const string AnteBetStraightValue = "AnteBet_straight";
        public const string AnteBetFlushValue = "AnteBet_flush";
        public const string AnteBetFullHouseValue = "AnteBet_fullHouse";
        public const string AnteBetFourKindsValue = "AnteBet_fourKind";
        public const string AnteBetStraightFlushValue = "AnteBet_straightFlush";
        public const string AnteBetRoyalFlushValue = "AnteBet_royalFlush";

        public static readonly TexasHoldemPokerOdds AnteBetHighCard = new TexasHoldemPokerOdds(AnteBetHighCardValue, "High Card");
        public static readonly TexasHoldemPokerOdds AnteBetOnePair = new TexasHoldemPokerOdds(AnteBetOnePairValue, "One Pair");
        public static readonly TexasHoldemPokerOdds AnteBetTwoPairs = new TexasHoldemPokerOdds(AnteBetTwoPairsValue, "Two Pairs");
        public static readonly TexasHoldemPokerOdds AnteBetThreeKinds = new TexasHoldemPokerOdds(AnteBetThreeKindsValue, "Three of a Kind");
        public static readonly TexasHoldemPokerOdds AnteBetStraight = new TexasHoldemPokerOdds(AnteBetStraightValue, "Straight");
        public static readonly TexasHoldemPokerOdds AnteBetFlush = new TexasHoldemPokerOdds(AnteBetFlushValue, "Flush");
        public static readonly TexasHoldemPokerOdds AnteBetFullHouse = new TexasHoldemPokerOdds(AnteBetFullHouseValue, "Full House");
        public static readonly TexasHoldemPokerOdds AnteBetFourKinds = new TexasHoldemPokerOdds(AnteBetFourKindsValue, "Four of a Kind");
        public static readonly TexasHoldemPokerOdds AnteBetStraightFlush = new TexasHoldemPokerOdds(AnteBetStraightFlushValue, "Straight Flush");
        public static readonly TexasHoldemPokerOdds AnteBetRoyalFlush = new TexasHoldemPokerOdds(AnteBetRoyalFlushValue, "Royal Flush");

        public TexasHoldemPokerOdds()
        {
        }

        public TexasHoldemPokerOdds(string value, string displayName)
            : base(value, displayName)
        {
        }
    }

}
