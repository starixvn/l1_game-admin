using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class DragonTigerFightBetHistory : BetHistory
    {
        public DragonTigerFightRoundResult RoundResult { get; set; }
        public IEnumerable<DragonTigerFightBetHistoryRecord> Bets { get; set; }
        public Dictionary<string, decimal> Odds { get; set; }
    }

    public class DragonTigerFightBetHistoryRecord : BetHistoryRecord
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}