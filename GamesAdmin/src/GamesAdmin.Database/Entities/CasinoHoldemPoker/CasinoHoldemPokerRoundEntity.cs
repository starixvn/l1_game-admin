using GamesAdmin.Core.Models;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("casinoholdempoker_phiens")]
    public class CasinoHoldemPokerRoundEntity : BaseRoundEntity
    {
        public CasinoHoldemPokerResultEntity Result { get; set; }

        public CombinationCard SettlementResult { get; set; }

        public DateTime RoundStartTime { get; set; }

        public bool Ended { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }
    }

    public class CasinoHoldemPokerResultEntity
    {
        public IEnumerable<PokerCard> Dealer { get; set; }
        public IEnumerable<PokerCard> CommunityCards { get; set; }
    }

    public class CombinationCard
    {
        public IEnumerable<PokerCard> Cards { get; set; }
        public IEnumerable<PokerCard> CombinationCards { get; set; }
        public bool? IsQualified { get; set; }
        [BsonElement("id")]
        public int HandRankId { get; set; }
} 
