using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class FootballDiceBetHistory : BetHistory
    {
        public FootballDiceRoundResult RoundResult { get; set; }
        public IEnumerable<FootballDiceBetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class FootballDiceBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}