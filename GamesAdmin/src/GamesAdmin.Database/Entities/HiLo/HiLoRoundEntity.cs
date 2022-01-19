using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("hilo_rounds")]
    public class HiLoRoundEntity : BaseRoundEntity
    {
        public decimal Amount { get; set; }

        public decimal Betwin { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }

        public int TotalTurns { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        public string Currency { get; set; }

        public string PartnerId { get; set; }

        public decimal Rate { get; set; }

        public bool Paid { get; set; }

        public IEnumerable<Card> Result { get; set; }

        public IEnumerable<string> SettlementResult { get; set; }
    }
}