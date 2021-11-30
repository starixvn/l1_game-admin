using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class CasinoHoldemPokerBetHistory
    {
        public long RoundId { get; set; }

        public CasinoHoldemPokerHandModel Player { get; set; }

        public CasinoHoldemPokerHandModel Dealer { get; set; }

        public IEnumerable<PokerCard> CommunityCards { get; set; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; set; }

        public decimal AA { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public bool Ended { get; set; }

        public bool IsFold { get; set; }

        public bool IsQualified { get; set; }

        public decimal Stake { get; set; }

        public decimal TotalWin { get; set; }

        public decimal BetWin { get; set; }
    }

    public class CasinoHoldemPokerBetSettlementResult
    {
        public decimal AA { get; set; }
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
    }

    public class CasinoHoldemPokerHandModel
    {
        private readonly string[] RankHandName = {
                        "DoesNotQualify",
                        "HighCard",
                        "Pair",
                        "TwoPairs",
                        "ThreeOfAKind",
                        "Straight",
                        "Flush",
                        "FullHouse",
                        "FourOfAKind",
                        "StraightFlush",
                        "RoyalFlush",
                    };
        public int Id { get; set; }
        public IEnumerable<PokerCard> Cards { get; set; }
        public IEnumerable<PokerCard> CombinationCards { get; set; }
        public string Rank => RankHandName[Id];
    }
}
