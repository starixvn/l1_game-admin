using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbo_phiens")]
    public class SicBoRoundEntity : BaseRoundEntity
    {
        public SicBoResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class SicBoResultEntity
    {
        [BsonElement("dice1")]
        public byte FirstDice { get; set; }

        [BsonElement("dice2")]
        public byte SecondDice { get; set; }

        [BsonElement("dice3")]
        public byte ThirdDice { get; set; }
    }

    public class SicBoSettlementResultEntity
    {
        public bool AnyTriple { get; set; }

        public bool Big { get; set; }

        public bool Small { get; set; }

        public bool Specific1 { get; set; }

        public bool Specific2 { get; set; }

        public bool Specific3 { get; set; }

        public bool Specific4 { get; set; }

        public bool Specific5 { get; set; }

        public bool Specific6 { get; set; }

        public bool Total4 { get; set; }

        public bool Total5 { get; set; }

        public bool Total6 { get; set; }

        public bool Total7 { get; set; }

        public bool Total8 { get; set; }

        public bool Total9 { get; set; }

        public bool Total10 { get; set; }

        public bool Total11 { get; set; }

        public bool Total12 { get; set; }

        public bool Total13 { get; set; }

        public bool Total14 { get; set; }

        public bool Total15 { get; set; }

        public bool Total16 { get; set; }

        public bool Total17 { get; set; }
    }
}