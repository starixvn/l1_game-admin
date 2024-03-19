using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballdice_bets")]
    public class FootballDiceBetEntity : BaseBetEntity
    {
        public decimal Home { get; set; }

        public decimal Away { get; set; }

        public decimal Draw { get; set; }
        
    }
}