using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_phiens")]
    [BsonIgnoreExtraElements]
    public class FishPrawnCrabRoundEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public long Number { get; set; }

        [BsonElement("dice1")]
        public int FirstDice { get; set; }

        [BsonElement("dice2")]
        public int SecondDice { get; set; }

        [BsonElement("dice3")]
        public int ThirdDice { get; set; }
    }
}