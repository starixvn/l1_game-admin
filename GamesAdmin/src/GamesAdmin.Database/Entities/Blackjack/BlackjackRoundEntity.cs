using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    public class BlackjackGameRoundEntity : BaseRoundEntity
    {
        public IEnumerable<CardEntity> Result { get; set; }

        public DateTime RoundStartTime { get; set; }

        public bool Ended { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public byte SettlementResult { get; set; }
    }

    [BsonCollection("blackjack_phiens")]
    public class BlackjackRoundEntity : BlackjackGameRoundEntity
    {
    }

    public class CardEntity
    {
        public string Rank { get; set; }

        public byte Suit { get; set; }
    }
}