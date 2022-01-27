using GamesAdmin.Core.Models.BetReport;
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.RoundResults
{
    public class BaseCasinoHoldemPokerResultControl
    {
        public HoldemPokerHandModel playerResult { get; set; }

        public HoldemPokerHandModel dealerResult { get; set; }

        public bool isQualified { get; set; }
    }

    public class HoldemPokerResultControl : BaseCasinoHoldemPokerResultControl
    {
        public IEnumerable<PokerCard> communityCards { get; set; }
    }
}