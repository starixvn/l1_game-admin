using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class LotteryBetHistory : BetHistory
    {
        public LotteryRoundResult RoundResult { get; set; }

        public IEnumerable<LotteryBetHistoryRecord> Bets { get; set; }

        public LotteryOdds Odds { get; set; }
    }

    public class LotteryBetHistoryRecord : BetHistoryRecord
    {
        public LotteryChoices Choices { get; set; }
    }

    public class LotteryChoices
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

    public class LotteryOdds
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