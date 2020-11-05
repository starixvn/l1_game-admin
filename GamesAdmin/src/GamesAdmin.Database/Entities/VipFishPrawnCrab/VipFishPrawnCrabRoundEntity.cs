using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("vipfishprawncrab_phiens")]
    public class VipFishPrawnCrabRoundEntity : BaseRoundEntity
    {
        public VipFishPrawnCrabRoundResultEntity Result { get; set; }

        public VipFishPrawnCrabSettlementResultEntity SettlementResult { get; set; }
    }

    public class VipFishPrawnCrabRoundResultEntity
    {
        [BsonElement("dice1")]
        public byte FirstDice { get; set; }

        [BsonElement("dice2")]
        public byte SecondDice { get; set; }

        [BsonElement("dice3")]
        public byte ThirdDice { get; set; }
    }

    public class VipFishPrawnCrabSettlementResultEntity
    {
        public bool Stag { get; set; }

        public bool Gourd { get; set; }

        public bool Rooster { get; set; }

        public bool Fish { get; set; }

        public bool Crab { get; set; }

        public bool Prawn { get; set; }

        public bool AnyTriple { get; set; }
    }
}