using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class KenoSoccerBetHistoryViewModel
    {
        public KenoSoccerRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<KenoSoccerBetHistoryRecordViewModel> Bets { get; set; }

    }

    public class KenoSoccerBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}