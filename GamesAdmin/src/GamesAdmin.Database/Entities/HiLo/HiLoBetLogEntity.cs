using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class HiLoBetLogEntity : BaseBetEntity
    {
        public int Order { get; set; }

        public decimal Amount { get; set; }

        public string Choice { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime EndTime { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }
}