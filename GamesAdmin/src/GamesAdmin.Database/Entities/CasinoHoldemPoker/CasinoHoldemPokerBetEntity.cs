using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("casinoholdempoker_cuocs")]
    public class CasinoHoldemPokerBetEntity : HoldemPokerBetEntity
    {
        public decimal Aa { get; set; }

        public decimal AaOdds { get; set; }

        public CasinoHoldemPokerBetSettlementResult SettlementResult { get; set; }
    }
}
