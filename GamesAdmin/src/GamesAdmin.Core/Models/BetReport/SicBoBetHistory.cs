using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
     public class SicBoBetHistory
    {
        public SicBoRoundResult RoundResult { get; set; }

        public IEnumerable<SicBoBetHistoryRecord> Bets { get; set; }
    }

    public class SicBoBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}