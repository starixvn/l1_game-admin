using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class BaseBetEntity : MiniGameBaseEntity
    {
        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("roundId")]
        public long Round { get; set; }

        public int? PlayerId { get; set; }


        public string SiteId { get; set; }

        public bool? FreeBet { get; set; }

        public string OperatorId { get; set; }

        public string Currency { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Rate { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BaseAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BaseRefund { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BaseBetWin { get; set; }

        public int ExchangeRateId { get; set; }
    }
}