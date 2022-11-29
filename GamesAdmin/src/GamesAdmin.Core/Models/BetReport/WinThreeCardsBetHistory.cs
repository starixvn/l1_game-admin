
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class WinThreeCardsBetHistory : BetHistory
    {
        public Dictionary<string, decimal> Odds { get; set; }

        public WinThreeCardsRoundResult RoundResult { get; set; }

        public IEnumerable<WinThreeCardsBetHistoryRecord> Bets { get; set; }
    }

    public class WinThreeCardsBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
