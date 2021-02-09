using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipsediewar_cuocs")]
    public class VipSeDieWarBetEntity : BaseBetEntity
    {
        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}