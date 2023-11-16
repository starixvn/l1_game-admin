using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{
    public class CrashGameRoundEntity : BaseRoundEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Result { get; set; }
    }
}
