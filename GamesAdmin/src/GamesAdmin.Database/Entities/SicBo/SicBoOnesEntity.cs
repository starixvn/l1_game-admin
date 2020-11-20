using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbo_ones")]
    public class SicBoOnesEntity : BaseBetEntity
    {
        public decimal TotalAmount { get; set; }
    }
}