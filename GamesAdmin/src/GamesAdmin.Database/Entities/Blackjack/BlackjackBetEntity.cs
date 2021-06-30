using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("blackjack_cuocs")]
    public class BlackjackBetEntity : BaseBetEntity
    {
        [BsonElement("id")]
        public long HandId { get; set; }

        public byte Pos { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public IEnumerable<CardEntity> Cards { get; set; }

        public IEnumerable<byte> SoftPoints { get; set; }

        public byte Total { get; set; }

        public long? SplitFrom { get; set; }

        public decimal Bet { get; set; }
    }
}