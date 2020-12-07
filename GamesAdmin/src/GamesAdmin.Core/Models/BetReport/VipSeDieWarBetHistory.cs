using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class VipSeDieWarBetHistory
    {
        public VipSeDieWarRoundResult RoundResult { get; set; }

        public IEnumerable<VipSeDieWarBetHistoryRecord> Bets { get; set; }
    }

    public class VipSeDieWarBetHistoryRecord
    {
        public long Round { get; set; }
        public DateTime Time { get; set; }
        public string Choice { get; set; }
        public decimal Amount { get; set; }
    }
}