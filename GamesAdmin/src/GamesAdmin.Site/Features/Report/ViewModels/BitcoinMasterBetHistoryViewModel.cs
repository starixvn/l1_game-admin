using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BitcoinMasterBetHistoryViewModel
    {
        public decimal? ExchangeRate { get; set; }

        public BitcoinMasterRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<BitcoinMasterBetHistoryRecordViewModel> Bets { get; set; }
    }

    public class BitcoinMasterBetHistoryRecordViewModel : BaseBetViewModel
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public bool Win { get; set; }

        public decimal GrossWin => Win ? BetWin + Amount - Refund : 0;
    }
}
