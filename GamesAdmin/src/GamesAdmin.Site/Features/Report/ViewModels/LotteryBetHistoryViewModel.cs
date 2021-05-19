using System;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.Report.ViewModels
{
    public class LotteryBetHistoryViewModel
    {
        public LotteryRoundResultViewModel RoundResult { get; set; }

        public IEnumerable<LotteryBetHistoryRecordViewModel> Bets { get; set; }

        public LotteryOddsViewModel Odds { get; set; }
    }

    public class LotteryBetHistoryRecordViewModel
    {
        public long Round { get; set; }

        public DateTime Time { get; set; }

        public LotteryChoicesViewModel Choices { get; set; }

        public string Name { get; set; }
    }

    public class LotteryChoicesViewModel
    {
        public Dictionary<string, decimal> FourSets { get; set; }

        public Dictionary<string, decimal> Special { get; set; }

        public Dictionary<string, decimal> Head { get; set; }

        public Dictionary<string, decimal> Tail { get; set; }

        public Dictionary<string, decimal> Parlay2 { get; set; }

        public Dictionary<string, decimal> Parlay3 { get; set; }

        public Dictionary<string, decimal> Parlay4 { get; set; }

        public Dictionary<string, decimal> TwentySevenSets { get; set; }

        public Dictionary<string, decimal> Others { get; set; }
    }

    public class LotteryOddsViewModel
    {
        public decimal FourSets { get; set; }

        public decimal Special { get; set; }
        public decimal Head { get; set; }
        public decimal Tail { get; set; }
        public decimal Parlay2 { get; set; }
        public decimal Parlay3 { get; set; }
        public decimal Parlay4 { get; set; }

        public decimal TwentySevenSets { get; set; }

        public decimal Others { get; set; }
    }
}