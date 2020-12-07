using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieWarBetHistoryViewModel
    {
        public VipSeDieWarRoundResultViewModel RoundResult { get; set; }

        public  IEnumerable<VipSeDieWarBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class VipSeDieWarBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}