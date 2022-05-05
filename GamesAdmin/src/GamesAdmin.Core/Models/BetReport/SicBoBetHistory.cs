using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SicBoBetHistory : BetHistory
    {
        public SicBoRoundResult RoundResult { get; set; }

        public IEnumerable<SicBoBetHistoryRecord> Bets { get; set; }
    }

    public class SicBoBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}