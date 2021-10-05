using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bitcoinmaster_cuocs")]
    public class BitcoinMasterBetEntity : BaseBetEntity
    {
        [BsonElement("tralai")]
        public decimal? Back { get; set; }

        public decimal? Bet { get; set; }

        public decimal Amount { get; set; }

        public decimal Refund { get; set; }

        public string Choice { get; set; }
    }
}