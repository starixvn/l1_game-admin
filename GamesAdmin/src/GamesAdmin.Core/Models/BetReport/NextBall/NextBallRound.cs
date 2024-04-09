using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class NextBallRound : RoundResult
    {
        public long DrawId { get; set; }

        public List<NextBallRoundResult> Result { get; set; }

        public List<NextBallSettlementResult> SettlementResult { get; set; }
    }

    public class NextBallRoundResult
    {
        public int Ball { get; set; }

        public long DrawId { get; set; }
    }

    public class NextBallSettlementResult
    {
        public bool Over { get; set; }

        public bool Under { get; set; }

        public bool Odd { get; set; }

        public bool Even { get; set; }

        public long DrawId { get; set; }
    }
}