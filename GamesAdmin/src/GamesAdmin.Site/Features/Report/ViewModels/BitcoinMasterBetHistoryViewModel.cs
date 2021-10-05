using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BitcoinMasterBetHistoryViewModel
    {
        public BitcoinMasterRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BitcoinMasterBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class BitcoinMasterBetHistoryRecordViewModel
    {
        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Bet { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }
    }
}
