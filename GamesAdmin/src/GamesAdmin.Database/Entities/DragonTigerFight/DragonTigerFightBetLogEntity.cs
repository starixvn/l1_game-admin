using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("dragontigerfight_betlogs")]
    public class DragonTigerFightBetLogEntity : BaseBetLogEntity
    {
        public decimal WinAmount { get; set; }

        public decimal Multiplier { get; set; }

        public string Action { get; set; }
    }
}