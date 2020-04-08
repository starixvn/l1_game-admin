using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("game_configs")]
    public class GameConfigEntity : MiniGameBaseEntity
    {
        public string name { get; set; }

        public bool enabled { get; set; }

        public bool botenabled { get; set; }

        public double minbet { get; set; }

        public double maxbet { get; set; }

        public double maxbot { get; set; }

        public int? disabledround { get; set; }

        [BsonElement("bot_minbet")]
        public double botMinBet { get; set; }

        [BsonElement("bot_maxbet")]
        public double botMaxBet { get; set; }

        [BsonElement("hour_maxbot")]
        public double[] hoursMaxBot { get; set; }
    }
}
