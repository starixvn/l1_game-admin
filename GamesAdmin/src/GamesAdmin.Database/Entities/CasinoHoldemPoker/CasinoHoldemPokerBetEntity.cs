using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("casinoholdempoker_cuocs")]
    public class CasinoHoldemPokerBetEntity : BaseBetEntity
    {
        public decimal AA { get; set; }

        public decimal Ante { get; set; }

        public decimal Call { get; set; }

        public bool IsFold { get; set; }
    }
}