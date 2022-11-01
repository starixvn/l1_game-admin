using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_rounds")]
    public class Win3CardsRoundEntity : BaseRoundEntity
    {
        public Win3CardsResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class Win3CardsResultEntity
    {
        public Hand Dragon { get; set; }

        public Hand Phoenix { get; set; }
    }

    public class Hand
    {
        [BsonElement("card1")]
        public CardEntity FirstCard { get; set; }

        [BsonElement("card2")]
        public CardEntity SecondCard { get; set; }

        [BsonElement("card3")]
        public CardEntity ThirdCard { get; set; }
    }
}
