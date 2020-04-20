using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public abstract class BaseRoundEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public long Number { get; set; }

        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}