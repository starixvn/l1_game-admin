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

        public IEnumerable<BlackjackCardEntity> Cards { get; set; }

        public byte Total { get; set; }

        public byte? SplitFrom { get; set; }

        public decimal Bet { get; set; }
    }
}