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
        public decimal Stag { get; set; }

        public decimal Gourd { get; set; }

        public decimal Rooster { get; set; }

        public decimal Fish { get; set; }

        public decimal Crab { get; set; }

        public decimal Prawn { get; set; }

        public decimal AnyTriple { get; set; }
    }
}