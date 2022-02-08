using System;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class BitcoinMasterBetHistory
    {
        public BitcoinMasterRoundResult RoundResult { get; set; }

        public IEnumerable<BitcoinMasterBetHistoryRecord> Bets { get; set; }
    }

    public class BitcoinMasterBetHistoryRecord
    {
        public DateTime Time { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public decimal BetWin { get; set; }

        public bool Win { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }

        public decimal BaseAmount { get; set; }

        public decimal BaseRefund { get; set; }

        public decimal BaseBetWin { get; set; }
    }
}