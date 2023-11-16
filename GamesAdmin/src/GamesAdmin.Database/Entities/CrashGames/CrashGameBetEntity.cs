using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{ 
    public class CrashGameBetEntity : BaseBetEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }
    }
}
