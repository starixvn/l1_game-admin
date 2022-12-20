using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BetHistoryViewModel
    {
        public bool ConvertToUSD { get; set; }

        public int BaseCurrencyMultiplier { get; set; }

        public bool FractionalOddsFormat { get; set; }
    }

    public class BetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }

        public decimal ConvertToBase(decimal value) => value * Rate;
    }
}