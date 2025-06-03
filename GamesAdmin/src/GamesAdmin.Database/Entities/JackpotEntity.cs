using System;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("jackpots")]
    public class JackpotEntity : Document
    {
        public int GameId { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Pool { get; set; }
        public string Prize { get; set; }

        public decimal InitAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Amount { get; set; }

        public decimal? WinningRatio { get; set; }

        public string WinnerName { get; set; }

        public string WinnerId { get; set; }

        public decimal ByBots { get; set; }

        public decimal ByMembers { get; set; }

        public long CreatedAt { get; set; }

        public long? HitAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? HitTime { get; set; }

        public int ExchangeRateId { get; set; } = 4308;

        public string Currency { get; set; } = "USD";

        public string OperatorId { get; set; } = "ONEworksVGCash";

        public string SiteName { get; set; }

        public decimal Rate { get; set; } = 1000;

        public string JackpotConfigId { get; set; }
    }
}
