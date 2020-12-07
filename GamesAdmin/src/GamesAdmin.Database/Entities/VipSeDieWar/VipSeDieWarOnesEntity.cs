using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipsediewar_ones")]
    public class VipSeDieWarOnesEntity : BaseBetEntity
    {
        public decimal TotalAmount { get; set; }
    }
}