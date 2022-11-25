using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoMatchBetHistory : BetHistory
    {
        public KenoMatchRoundResult RoundResult { get; set; }

        public IEnumerable<KenoMatchBetHistoryRecord> Bets { get; set; }

        public bool FractionalOddsFormat { get; set; }
    }

    public class KenoMatchBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}