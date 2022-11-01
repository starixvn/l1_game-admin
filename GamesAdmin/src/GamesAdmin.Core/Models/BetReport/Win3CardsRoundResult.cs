
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class Win3CardsRoundResult : RoundResult
    {
        public Win3CardsResult Result { get; set; }

        public Win3CardsSettlementResult SettlementResult { get; set; }
    }

    public class Win3CardsResult
    {
        public IEnumerable<Card> Dragon { get; set; }

        public IEnumerable<Card> Phoenix { get; set; }
    }

    public class Win3CardsSettlementResult
    {
        public bool Dragon { get; set; }

        public bool Phoenix { get; set; }

        public bool Pair8Plus { get; set; }

        public bool Straight { get; set; }

        public bool FLush { get; set; }

        public bool StraightFlush { get; set; }

        public bool ThreeKind { get; set; }
    }
}
