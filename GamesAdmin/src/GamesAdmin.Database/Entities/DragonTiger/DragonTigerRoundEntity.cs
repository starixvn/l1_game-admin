using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("minidragontiger_phiens")]
    public class DragonTigerRoundEntity : BaseRoundEntity
    {
        public Card Dragon { get; set; }

        public Card Tiger { get; set; }

        public DragonTigerResult Result { get; set; }
    }

    public class DragonTigerResult
    {
        public Card Dragon { get; set; }

        public Card Tiger { get; set; }
    }

    public class Card
    {
        [BsonElement("card")]
        public byte Number { get; set; }

        public byte Type { get; set; }
    }
}