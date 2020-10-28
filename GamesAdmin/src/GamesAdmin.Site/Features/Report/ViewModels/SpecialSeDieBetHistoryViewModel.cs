using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class SpecialSeDieBetHistoryViewModel
    {
        public SeDieRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<SpecialSeDieBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class SpecialSeDieBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }
    }
}