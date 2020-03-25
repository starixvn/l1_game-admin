using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("game_configs")]
    public class GameConfigEntity : Document
    {
        public string name { get; set; }

        public bool enabled { get; set; }

        public bool botenabled { get; set; }

        public double minbet { get; set; }

        public double maxbet { get; set; }

        public double maxbot { get; set; }

        public int? disabledround { get; set; }
    }
}
