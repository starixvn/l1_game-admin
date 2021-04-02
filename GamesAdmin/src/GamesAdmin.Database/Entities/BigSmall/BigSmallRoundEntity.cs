using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_phiens")]
    [BsonIgnoreExtraElements]
    public class BigSmallRoundEntity : BaseRoundEntity
    {
        public int? Dice1 { get; set; }

        public int? Dice2 { get; set; }

        public int? Dice3 { get; set; }

        public BigSmallResult Result { get; set; }
    }

    public class BigSmallResult
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}