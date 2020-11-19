using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sicbo_cuocs")]
    public class SicBoBetEntity : MiniGameBaseEntity
    {
        [BsonElement("phien")]
        public long Round { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int? MemberId { get; set; }

        public string SiteId { get; set; }

        public decimal? AnyTriple { get; set; }

        public decimal? Big { get; set; }

        public decimal? Small { get; set; }

        public decimal? Specific1 { get; set; }

        public decimal? Specific2 { get; set; }

        public decimal? Specific3 { get; set; }

        public decimal? Specific4 { get; set; }

        public decimal? Specific5 { get; set; }

        public decimal? Specific6 { get; set; }

        public decimal? Total4 { get; set; }

        public decimal? Total5 { get; set; }

        public decimal? Total6 { get; set; }

        public decimal? Total7 { get; set; }

        public decimal? Total8 { get; set; }

        public decimal? Total9 { get; set; }

        public decimal? Total10 { get; set; }

        public decimal? Total11 { get; set; }

        public decimal? Total12 { get; set; }

        public decimal? Total13 { get; set; }

        public decimal? Total14 { get; set; }

        public decimal? Total15 { get; set; }

        public decimal? Total16 { get; set; }

        public decimal? Total17 { get; set; }
    }
}