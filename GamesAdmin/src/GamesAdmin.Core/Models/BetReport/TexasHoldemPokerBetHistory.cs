using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class TexasHoldemPokerBetHistory
    {
        public long RoundId { get; set; }

        public CasinoHoldemPokerHandModel Player { get; set; }

        public CasinoHoldemPokerHandModel Dealer { get; set; }

        public IEnumerable<PokerCard> CommunityCards { get; set; }

        public TexasHoldemPokerBetSettlementResult SettlementResult { get; set; }

        public string AnteOdds { get; set; }

        public string CallOdds { get; set; }

        public string TurnOdds { get; set; }

        public string RiverOdds { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public decimal Turn { get; set; }

        public decimal River { get; set; }

        public bool Ended { get; set; }

        public bool IsFold { get; set; }

        public bool IsQualified { get; set; }

        public decimal Stake { get; set; }

        public decimal TotalWin { get; set; }

        public decimal BetWin { get; set; }

        public DateTime Time { get; set; }

        public DateTime CallTime { get; set; }

        public DateTime TurnTime { get; set; }

        public DateTime RiverTime { get; set; }
    }

    public class TexasHoldemPokerBetSettlementResult
    {
        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public decimal Turn { get; set; }

        public decimal River { get; set; }
    }
}
