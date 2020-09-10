using System;

namespace GamesAdmin.Core.Models.BetReport
{
    public class VipSeDieBetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}