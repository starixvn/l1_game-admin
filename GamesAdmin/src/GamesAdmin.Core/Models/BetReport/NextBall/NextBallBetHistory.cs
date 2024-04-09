using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class NextBallBetHistory : BetHistory
    {
        public NextBallRound RoundResult { get; set; }
        public IEnumerable<NextBallBetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class NextBallBetHistoryRecord : BetHistoryRecord
    {
        public long DrawId { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}