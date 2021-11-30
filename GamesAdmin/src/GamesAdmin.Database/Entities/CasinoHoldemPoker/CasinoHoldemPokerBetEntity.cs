using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("casinoholdempoker_cuocs")]
    public class CasinoHoldemPokerBetEntity : BaseBetEntity
    {
        public decimal Aa { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public bool IsFold { get; set; }

        public string Currency { get; set; }

        public IEnumerable<PokerCard> Cards { get; set; }

        public CombinationCard Result { get; set; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; set; }
    }
}
