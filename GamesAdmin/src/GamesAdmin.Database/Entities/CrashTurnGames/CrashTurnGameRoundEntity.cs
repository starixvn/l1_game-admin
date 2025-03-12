using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GamesAdmin.Database.Entities
{
    public class CrashTurnGameRoundEntity : BaseRoundEntity
    {
        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }

        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public string OperatorId { get; set; }

        public string Currency { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Result { get; set; }
        
        public decimal Rate { get; set; }
    }
}
