using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.ExchangeRates
{
    [BsonCollection("exchange_rates")]
    public class ExchangeRateEntity : Document
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        public int ExId { get; set; }

        public string Source { get; set; }

        public Dictionary<string, double> Code { get; set; }
    }
}