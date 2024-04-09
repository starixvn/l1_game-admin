using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BacBall2BetHistory : BetHistory
    {
        public BacBall2Round RoundResult { get; set; }
        public IEnumerable<BacBall2BetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }  

    public class BacBall2BetHistoryRecord : BetHistoryRecord
    {
        public long DrawId { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
