using GamesAdmin.Core.Models;
using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("texasholdempoker_cuocs")]
    public class TexasHoldemPokerBetEntity : BaseBetEntity
    {
        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public decimal Turn { get; set; }

        public decimal River { get; set; }

        public bool IsFold { get; set; }

        public IEnumerable<PokerCard> Cards { get; set; }

        public CombinationCard Result { get; set; }

        public TexasHoldemPokerBetSettlementResult SettlementResult { get; set; }
    }

    public class TexasHoldemPokerBetSettlementResult
    {
        public decimal Ante { get; set; }
        public decimal Call { get; set; }
        public decimal Turn { get; set; }
        public decimal River { get; set; }
    }
}
