using System;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class VipSeDieBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}