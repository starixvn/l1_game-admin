using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoPowerBetHistory : BetHistory
    {
        public KenoPowerRoundResult RoundResult { get; set; }

        public IEnumerable<KenoPowerBetHistoryRecord> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoPowerBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}