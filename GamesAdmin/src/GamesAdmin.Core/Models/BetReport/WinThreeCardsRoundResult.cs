
using System.Collections.Generic;

namespace GamesAdmin.Core.Models.BetReport
{
    public class WinThreeCardsRoundResult : RoundResult
    {
        public IEnumerable<DragonTigerCardResult> Dragon { get; set; }

        public IEnumerable<DragonTigerCardResult> Phoenix { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }

        public int DragonHandRankId { get; set; }

        public int PhoenixHandRankId { get; set; }
    }
}
