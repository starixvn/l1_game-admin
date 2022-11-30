using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class FanTanBetHistory : BetHistory
    {
        public FanTanRoundResult RoundResult { get; set; }

        public FanTanBetHistoryRecord Bet { get; set; }
    }

    public class FanTanBetHistoryRecord : BetHistoryRecord
    {
        public Dictionary<string, decimal> Odds { get; set; }
        public decimal BetWin { get; set; }

        public decimal Odd { get; set; }
        public decimal Even { get; set; }

        public decimal Fan1 { get; set; }
        public decimal Fan2 { get; set; }
        public decimal Fan3 { get; set; }
        public decimal Fan4 { get; set; }

        public decimal Kwok12 { get; set; }
        public decimal Kwok23 { get; set; }
        public decimal Kwok34 { get; set; }
        public decimal Kwok41 { get; set; }

        public decimal Chun123 { get; set; }
        public decimal Chun124 { get; set; }
        public decimal Chun134 { get; set; }
        public decimal Chun234 { get; set; }
    }
}