using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoPowerBetHistory
    {
        public KenoPowerRoundResult RoundResult { get; set; }

        public IEnumerable<KenoPowerBetHistoryRecord> Bets { get; set; }

        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class KenoPowerBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}