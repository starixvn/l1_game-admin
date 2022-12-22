using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class WinThreeCardsBetHistory : BetHistory
    {
        public Dictionary<string, decimal> Odds { get; set; }

        public WinThreeCardsRoundResult RoundResult { get; set; }

        public WinThreeCardsBetRecord Bet { get; set; }
    }

    public class WinThreeCardsRoundResult : RoundResult
    {
        public WinThreeCardsResult Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class WinThreeCardsResult
    {
        public WinThreeCardsHandResult Dragon { get; set; }
        public WinThreeCardsHandResult Phoenix { get; set; }
    }

    public class WinThreeCardsHandResult
    {
        private readonly string[] RankHandName = {
                        "High Card",
                        "Pair",
                        "Straight",
                        "Flush",
                        "Straight Flush",
                        "Three of a Kind",
                        "Killer 235",
                    };

        public string HandRank => HandRankId > 0 ? RankHandName[HandRankId - 1] : string.Empty;
        public IEnumerable<PokerCard> Cards { get; set; }
        public byte HandRankId { get; set; }
        public int CardRankId { get; set; }
    }
}