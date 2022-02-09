using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("texasholdempoker_cuocs")]
    public class TexasHoldemPokerBetEntity : HoldemPokerBetEntity
    {
        public decimal Turn { get; set; }

        public decimal TurnOdds { get; set; }

        public decimal River { get; set; }

        public decimal RiverOdds { get; set; }

        public DateTime TurnTime { get; set; }

        public DateTime RiverTime { get; set; }

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
