using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("colorgame_rounds")]
    public class ColorGameRoundEntity : BaseRoundEntity
    {
        public ColorGameRoundResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class ColorGameRoundResultEntity
    {
        [BsonElement("dice1")]
        public byte Dice1 { get; set; }

        [BsonElement("dice2")]
        public byte Dice2 { get; set; }

        [BsonElement("dice3")]
        public byte Dice3 { get; set; }
    }

    public class ColorGameSettlementResultEntity
    {
        public bool Yellow { get; set; }

        public bool Red { get; set; }

        public bool Pink { get; set; }

        public bool White { get; set; }

        public bool Blue { get; set; }

        public bool Green { get; set; }

        public bool Hot { get; set; }

        public bool Cold { get; set; }
    }
}