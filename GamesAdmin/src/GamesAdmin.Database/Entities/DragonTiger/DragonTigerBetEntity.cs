using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("minidragontiger_cuocs")]
    public class DragonTigerBetEntity : MiniGameBaseEntity
    {
        public int Dragon { get; set; }

        public int Tie { get; set; }

        public int Tiger { get; set; }

        [BsonElement("thanhtoan")]
        public bool Paid { get; set; }

        public bool BigWin { get; set; }

        public decimal Betwin { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        [BsonElement("phien")]
        public long Round { get; set; }

        public bool Red { get; set; }

        public int? MemberId { get; set; }
    }
}