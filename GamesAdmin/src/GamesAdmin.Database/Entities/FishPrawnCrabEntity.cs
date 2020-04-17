using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("baucua_cuocs")]
    public class FishPrawnCrabBetEntity : MiniGameBaseEntity
    {
        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        [BsonElement("bigWin")]
        public bool Win { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        [BsonElement("0")]
        public decimal Stag { get; set; }

        [BsonElement("1")]
        public decimal Gourd { get; set; }

        [BsonElement("2")]
        public decimal Rooster { get; set; }

        [BsonElement("3")]
        public decimal Fish { get; set; }

        [BsonElement("4")]
        public decimal Crab { get; set; }

        [BsonElement("5")]
        public decimal Prawn { get; set; }

        public bool Red { get; set; }
    }

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