using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class HiLoBetHistoryViewModel : BetHistoryViewModel
    {
        public HiLoRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<HiLoBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class HiLoBetHistoryRecordViewModel : BetHistoryRecordViewModel
    {
        public int Order { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Betwin { get; set; }

        public IEnumerable<CardViewModel> Cards { get; set; }
    }
}