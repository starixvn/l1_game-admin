using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class OddEvenBetHistoryViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public decimal BetWin { get; set; }

        public decimal Refund { get; set; }

        public OddEvenBetResultViewModel BetResult { get; set; }

        public OddEvenBetChoiceViewModel Choice { get; set; }
    }

    public class OddEvenBetChoiceViewModel
    {
        public bool Even { get; set; }

        public decimal Amount { get; set; }
    }

    public class OddEvenBetResultViewModel 
    {
        public IEnumerable<byte> Dices { get; set; }

        public int Total => Dices.Sum(x => x);

    }
}
