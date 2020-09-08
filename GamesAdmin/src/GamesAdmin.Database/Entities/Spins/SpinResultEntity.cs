using System;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.Spins
{
    [BsonCollection("spin_results")]
    public class SpinResultEntity : MiniGameBaseEntity
    {
        [BsonElement("id")]
        public int RoundId { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int MemberId { get; set; }

        public byte Result { get; set; }

        public DateTime Time { get; set; }
    }
}
