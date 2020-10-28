using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
     public class SpecialSeDieBetHistory
    {
        public SeDieRoundResult RoundResult { get; set; }

        public IEnumerable<SpecialSeDieBetHistoryRecord> Bets { get; set; }
    }

    public class SpecialSeDieBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }
    }
}