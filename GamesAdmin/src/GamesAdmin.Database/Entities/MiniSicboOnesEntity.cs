using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("taixiu_ones")]
    public class MiniSicboOnesEntity : Document
    {
        public bool red { get; set; }

        public decimal bet { get; set; }

        public int phien { get; set; }

        public bool select { get; set; }

        public double betwin { get; set; }

        public bool win { get; set; }

        public decimal tralai { get; set; }

        public decimal thuong { get; set; }

        public string uid { get; set; }

        public bool taixiu { get; set; }
    }
}
