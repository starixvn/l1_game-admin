using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class HoldemPokerBetHistory
    {
        public bool ConvertToUSD { get; set; }

        public long RoundId { get; set; }

        public HoldemPokerHandModel Player { get; set; }

        public HoldemPokerHandModel Dealer { get; set; }

        public IEnumerable<PokerCard> CommunityCards { get; set; }

        public decimal Ante { get; set; }

        public string AnteOdds { get; set; }

        public decimal Call { get; set; }

        public string CallOdds { get; set; }

        public bool Ended { get; set; }

        public bool IsFold { get; set; }

        public bool IsQualified { get; set; }

        public decimal Stake { get; set; }

        public decimal TotalWin { get; set; }

        public decimal BetWin { get; set; }

        public DateTime Time { get; set; }

        public DateTime CallTime { get; set; }

        public string Currency { get; set; }

        public decimal Rate { get; set; }

        public decimal ConvertToBase(decimal value) => value * Rate;
    }

    public class HoldemPokerHandModel
    {
        private readonly string[] RankHandName = {
                        "Does Not Qualify",
                        "High Card",
                        "Pair",
                        "Two Pair",
                        "Three Of A Kind",
                        "Straight",
                        "Flush",
                        "Full House",
                        "Four Of A Kind",
                        "Straight Flush",
                        "Royal Flush",
                    };
        public int Id { get; set; }
        public IEnumerable<PokerCard> Cards { get; set; }
        public IEnumerable<PokerCard> CombinationCards { get; set; }
        public string Rank => RankHandName[Id];
    }
}
