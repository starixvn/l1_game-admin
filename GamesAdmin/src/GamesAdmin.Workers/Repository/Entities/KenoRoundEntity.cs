using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Workers.Repository.Entities
{
    public class KenoRoundEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("id")]
        public long RoundNo { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        public IEnumerable<byte> Result { get; set; }

        public SettlementResult SettlementResult { get; set; }
    }

    public class SettlementResult
    {
        public bool Big { get; set; }
        public bool Small { get; set; }
        public bool Odd { get; set; }
        public bool Even { get; set; }
        public bool Gold { get; set; }
        public bool Wood { get; set; }
        public bool Water { get; set; }
        public bool Fire { get; set; }
        public bool Earth { get; set; }
    }
}