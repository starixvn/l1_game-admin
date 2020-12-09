using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SicBoBetHistoryViewModel
    {
        public SicBoRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<SicBoBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class SicBoBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}