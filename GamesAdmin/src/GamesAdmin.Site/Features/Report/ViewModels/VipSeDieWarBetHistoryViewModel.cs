using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieWarBetHistoryViewModel : BetHistoryViewModel
    {
        public VipSeDieWarRoundResultViewModel RoundResult { get; set; }

        public  IEnumerable<VipSeDieWarBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class VipSeDieWarBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}