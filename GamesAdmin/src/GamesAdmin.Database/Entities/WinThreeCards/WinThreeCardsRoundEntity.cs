using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("winthreecards_rounds")]
    public class WinThreeCardsRoundEntity : BaseRoundEntity
    {
        public WinThreeCardsResultEntity Result { get; set; }

        public Dictionary<string, bool> SettlementResult { get; set; }
    }

    public class WinThreeCardsResultEntity
    {
        public WinThreeCardsHandResultEntity Dragon { get; set; }

        public WinThreeCardsHandResultEntity Phoenix { get; set; }
    }

    public class WinThreeCardsHandResultEntity
    {
        public WinThreeCardsHandEntity Cards { get; set; }

        public int HandRankId { get; set; }

        public int CardRankId { get; set; }
    }

    public class WinThreeCardsHandEntity
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
        public byte Rank { get; set; }

        public byte Suit { get; set; }
    }
}