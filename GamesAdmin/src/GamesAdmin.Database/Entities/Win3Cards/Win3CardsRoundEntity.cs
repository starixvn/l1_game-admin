using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_rounds")]
    public class Win3CardsRoundEntity : BaseRoundEntity
    {
        public Win3CardsResult Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class Win3CardsResult
    {
        public Win3CardsHandResult Dragon { get; set; }

        public Win3CardsHandResult Phoenix { get; set; }
    }

    public class Win3CardsHandResult
    {
        [BsonElement("card1")]
        public Win3CardsCardEntity FirstCard { get; set; }

        [BsonElement("card2")]
        public Win3CardsCardEntity SecondCard { get; set; }

        [BsonElement("card3")]
        public Win3CardsCardEntity ThirdCard { get; set; }
    }

    public class Win3CardsCardEntity
    {
        [BsonElement("rank")]
        public byte Number { get; set; }

        [BsonElement("suit")]
        public byte Type { get; set; }
    }
}
