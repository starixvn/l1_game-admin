using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class CasinoHoldemPokerBetHistory
    {
        public bool ConvertToUSD { get; set; }

        public long RoundId { get; set; }

        public CasinoHoldemPokerHandModel Player { get; set; }

        public CasinoHoldemPokerHandModel Dealer { get; set; }

        public IEnumerable<PokerCard> CommunityCards { get; set; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; set; }

        public string AaOdds { get; set; }

        public string AnteOdds { get; set; }

        public string CallOdds { get; set; }

        public decimal AA { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }

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

    public class CasinoHoldemPokerBetSettlementResult
    {
        public decimal Aa { get; set; }
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
    }

    public class CasinoHoldemPokerHandModel
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
