using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Database.Entities.Plinko
{
    public class BasePlinkoRoundEntity : BaseRoundEntity
    {
        public bool Paid { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal BetWin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public string PlayerId { get; set; }

        public string SiteId { get; set; }

        public bool? FreeBet { get; set; }

        public string OperatorId { get; set; }

        public string Currency { get; set; }

        [BsonRepresentation(BsonType.Decimal128, AllowTruncation = true)]
        public decimal Rate { get; set; }
    }
}
