using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("txturbo_cuocs")]
    public class BigSmallTurboBetEntity : BaseBetEntity
    {
        [BsonElement("taixiu")]
        public bool BigSmall { get; set; }

        [BsonElement("tralai")]
        public decimal Back { get; set; }

        public decimal Bet { get; set; }

        public bool Select { get; set; }

        public decimal Refund { get; set; }

        public string Choice { get; set; }

        public decimal Amount { get; set; }
    }
}