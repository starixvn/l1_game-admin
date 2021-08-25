using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoPowerBetHistoryViewModel
    {
        public KenoPowerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoPowerBetHistoryRecordViewModel> Bets { get; set; }

        public Dictionary<string, bool> Odds { get; set; }
    }

    public class KenoPowerBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}