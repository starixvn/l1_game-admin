using System;
using System.Collections.Generic;
using GamesAdmin.Core.Enumeration.BetChoices;

namespace GamesAdmin.Core.Models.BetReport
{
    public class KenoBetHistory : BetHistory
    {
        public IEnumerable<KenoBetHistoryRecord> Bets { get; set; }
    }
    public class KenoBetHistoryRecord : BetHistoryRecord
    {
        public KenoBetChoice Choice { get; set; }

        public decimal Amount { get; set; }
    }
}
