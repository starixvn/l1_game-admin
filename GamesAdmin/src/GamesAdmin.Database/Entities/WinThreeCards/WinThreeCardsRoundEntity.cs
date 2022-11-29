using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_rounds")]
    public class WinThreeCardsRoundEntity : BaseRoundEntity
    {
        public WinThreeCardsResult Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class WinThreeCardsResult
    {

        public WinThreeCardsHandResult Dragon { get; set; }

        public WinThreeCardsHandResult Phoenix { get; set; }
    }

    public class WinThreeCardsHandResult
    {
        public WinThreeCardsHand Cards { get; set; }

        public int HandRankId { get; set; }
    }

    public class WinThreeCardsHand
    {
        [BsonElement("card1")]
        public WinThreeCardsCardEntity FirstCard { get; set; }

        [BsonElement("card2")]
        public WinThreeCardsCardEntity SecondCard { get; set; }

        [BsonElement("card3")]
        public WinThreeCardsCardEntity ThirdCard { get; set; }
    }

    public class WinThreeCardsCardEntity
    {
        [BsonElement("rank")]
        public byte Number { get; set; }

        [BsonElement("suit")]
        public byte Type { get; set; }
    }
}
