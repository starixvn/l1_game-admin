using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class TexasHoldemPokerBetHistory : HoldemPokerBetHistory
    {
        public TexasHoldemPokerBetSettlementResult SettlementResult { get; set; }

        public string TurnOdds { get; set; }

        public string RiverOdds { get; set; }

        public decimal Turn { get; set; }

        public decimal River { get; set; }

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
