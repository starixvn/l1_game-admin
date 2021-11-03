using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoMatchBetHistory
    {
        public KenoMatchRoundResult RoundResult { get; set; }

        public IEnumerable<KenoMatchBetHistoryRecord> Bets { get; set; }
    }

    public class KenoMatchBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}