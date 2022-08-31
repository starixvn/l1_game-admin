using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class FruitSlotBetHistory : BetHistory
    {
        public FruitSlotRoundResult RoundResult { get; set; }
        public IEnumerable<FruitSlotBetHistoryRecord> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }
    public class FruitSlotBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public IEnumerable<string> SubChoices { get; set; }

        public decimal Amount { get; set; }
    }
}
