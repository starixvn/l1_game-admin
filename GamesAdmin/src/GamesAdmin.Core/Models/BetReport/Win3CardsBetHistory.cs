
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class Win3CardsBetHistory : BetHistory
    {
        public Dictionary<string, decimal> Odds { get; set; }

        public Win3CardsRoundResult RoundResult { get; set; }

        public IEnumerable<Win3CardsBetHistoryRecord> Bets { get; set; }
    }

    public class Win3CardsBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
