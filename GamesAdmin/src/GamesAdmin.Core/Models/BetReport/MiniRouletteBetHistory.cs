using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Core.Models.BetReport
{
    public class MiniRouletteBetHistory : BetHistory
    {
        public MiniRouletteRoundResult RoundResult { get; set; }

        public IEnumerable<MiniRouletteBetHistoryRecord> Bets { get; set; }
    }

    public class MiniRouletteBetHistoryRecord : BetHistoryRecord
    {
        public Dictionary<string, decimal> StraightUp { get; set; }

        public Dictionary<string, decimal> Split { get; set; }

        public Dictionary<string, decimal> Street { get; set; }

        public Dictionary<string, decimal> Triangle { get; set; }

        public Dictionary<string, decimal> Corner { get; set; }

        public Dictionary<string, decimal> FourNumbers { get; set; }

        public Dictionary<string, decimal> Line { get; set; }

        public Dictionary<string, decimal> Column { get; set; }

        public Dictionary<string, decimal> Dozen { get; set; }

        public Dictionary<string, decimal> RedBlack { get; set; }

        public Dictionary<string, decimal> OddEven { get; set; }

        public Dictionary<string, decimal> BigSmall { get; set; }
    }
}
