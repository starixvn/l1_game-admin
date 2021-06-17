using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("threecards_cuocs")]
    public class ThreeCardsBetEntity : BaseBetEntity
    {
        public decimal Red { get; set; }
        public decimal Black { get; set; }
        public decimal Diamonds { get; set; }
        public decimal Hearts { get; set; }
        public decimal Spades { get; set; }
        public decimal Clubs { get; set; }
    }
}