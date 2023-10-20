using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BandarQiuQiuBetHistory : BetHistory
    {
        public BandarQiuQiuRoundResult RoundResult { get; set; }
        public IEnumerable<BandarQiuQiuBetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }  

    public class BandarQiuQiuBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
