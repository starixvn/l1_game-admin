using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class SlotRNGBetHistory : BetHistory
    {
        public SlotRNGRoundResult RoundResult { get; set; }
        public IEnumerable<SlotRNGBetHistoryRecord> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class SlotRNGBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public IEnumerable<string> SubChoices { get; set; }

        public decimal Amount { get; set; }
    }
}