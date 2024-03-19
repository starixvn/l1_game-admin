using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("footballdice_rounds")]
    [BsonIgnoreExtraElements]
    public class FootballDiceRoundEntity : BaseRoundEntity
    {
        public FootballDiceResultEntity Result { get; set; }

        public FootballDiceSettlementResultEntity SettlementResult { get; set; }
    }

    public class FootballDiceResultEntity
    {
        public decimal[] Home { get; set; }
        public decimal[] Away { get; set; }

    }

    public class FootballDiceSettlementResultEntity
    {
        public bool Home { get; set; }

        public bool Away { get; set; }
        public bool Draw { get; set; }
    }
}