using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class BigSmallBetHistoryViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public decimal BetWin { get; set; }

        public decimal Refund { get; set; }

        public BigSmallBetResultViewModel BetResult { get; set; }

        public BigSmallBetChoiceViewModel Choice { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal? ConvertByExchangeRate(decimal? value)
        {
            var exchangeRate = ExchangeRate == 0 ? 1 : ExchangeRate;

            return value != null ? value / exchangeRate : null;
        }
    }

    public class BigSmallBetChoiceViewModel
    {
        public bool Big { get; set; }

        public decimal Amount { get; set; }
    }

    public class BigSmallBetResultViewModel
    {
        public IEnumerable<byte> Dices { get; set; }

        public int Total => Dices.Sum(x => x);

    }
}
