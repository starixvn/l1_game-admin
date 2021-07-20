using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoSoccerBetHistory
    {
        public KenoSoccerRoundResult RoundResult { get; set; }

        public IEnumerable<KenoSoccerBetHistoryRecord> Bets { get; set; }
    }

    public class KenoSoccerBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}