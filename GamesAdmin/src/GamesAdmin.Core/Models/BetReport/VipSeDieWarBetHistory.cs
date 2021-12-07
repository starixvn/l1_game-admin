using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class VipSeDieWarBetHistory : BetHistory
    {
        public VipSeDieWarRoundResult RoundResult { get; set; }

        public IEnumerable<VipSeDieWarBetHistoryRecord> Bets { get; set; }
    }

    public class VipSeDieWarBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }
        public decimal Amount { get; set; }
    }
}