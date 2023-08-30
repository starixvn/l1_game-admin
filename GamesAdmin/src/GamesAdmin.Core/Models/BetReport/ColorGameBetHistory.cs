using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class ColorGameBetHistory : BetHistory
    {
        public ColorGameRoundResult RoundResult { get; set; }
        public IEnumerable<ColorGameBetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class ColorGameBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}