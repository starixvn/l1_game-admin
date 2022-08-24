using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class UnbalancedBigSmallBetHistory : BetHistory
    {
        public BigSmallRoundResult RoundResult { get; set; }

        public IEnumerable<UnbalancedBigSmallBetHistoryRecord> Bets { get; set; }
    }

    public class UnbalancedBigSmallBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}