using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("chanleturbo_cuocs")]
    public class OddEvenTurboBetEntity : BaseBetEntity
    {
        [BsonElement("tralai")]
        public decimal Back { get; set; }

        public decimal Bet { get; set; }

        public bool Select { get; set; }
    }
}