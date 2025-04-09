using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{ 
    public class CrashGameBetLogEntity : BaseBetLogEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal WinAmount { get; set; }

        public decimal Multiplier { get; set; }

        public string Action { get; set; }
        
        public byte? BetKey{ get; set; }
    }
}
