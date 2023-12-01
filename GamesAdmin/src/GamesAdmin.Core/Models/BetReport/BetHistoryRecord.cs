using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BetHistory
    {
        public bool FractionalOddsFormat { get; set; }

        public string CurrencyName { get; set; }
    }

    public class BetHistoryRecord
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }
    }
}