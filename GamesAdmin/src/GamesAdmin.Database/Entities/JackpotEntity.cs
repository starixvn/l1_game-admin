using System;
using System.Collections.Generic;
using System.Text;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("jackpots")]
    public class JackpotEntity : Document
    {
        public byte GameId { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Pool { get; set; }

        public decimal InitAmount { get; set; }

        public decimal Amount { get; set; }

        public decimal? WinningRatio { get; set; }

        public string WinnerName { get; set; }

        public int? WinnerId { get; set; }

        public decimal ByBots { get; set; }

        public decimal ByMembers { get; set; }

        public long CreatedAt { get; set; }

        public long? HitAt { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedTime { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? HitTime { get; set; }
    }
}
