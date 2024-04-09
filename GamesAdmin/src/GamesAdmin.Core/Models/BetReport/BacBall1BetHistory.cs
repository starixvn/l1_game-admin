using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BacBall1BetHistory : BetHistory
    {
        public BacBall1Round RoundResult { get; set; }
        public IEnumerable<BacBall1BetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }  

    public class BacBall1BetHistoryRecord : BetHistoryRecord
    {
        public long DrawId { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
