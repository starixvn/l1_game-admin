using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoMatchBetHistoryViewModel
    {
        public KenoMatchRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoMatchBetHistoryRecordViewModel> Bets { get; set; }

    }

    public class KenoMatchBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}