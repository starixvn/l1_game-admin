using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanle_cuocs")]
    public class OddEvenBetEntity : BaseBetEntity
    {
        public decimal? Back { get; set; }

        public decimal? Bet { get; set; }

        public bool? Select { get; set; }

        public decimal Refund { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}