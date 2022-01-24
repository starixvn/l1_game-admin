using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class HiLoBetHistory : BetHistory
    {
        public HiLoRoundResult RoundResult { get; set; }

        public IEnumerable<HiLoBetHistoryRecord> Bets { get; set; }
    }

    public class HiLoBetHistoryRecord : BetHistoryRecord
    {
        public int Order { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}