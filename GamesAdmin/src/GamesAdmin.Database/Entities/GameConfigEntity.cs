using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("game_configs")]
    public class GameConfigEntity : MiniGameBaseEntity
    {
        public string name { get; set; }

        public bool Enabled { get; set; }

        public bool Botenabled { get; set; }

        public double Minbet { get; set; }

        public double Maxbet { get; set; }

        public double Maxbot { get; set; }

        public int? Disabledround { get; set; }

        [BsonElement("bot_minbet")]
        public double BotMinBet { get; set; }

        [BsonElement("bot_maxbet")]
        public double BotMaxBet { get; set; }

        [BsonElement("hour_maxbot")]
        public double[] HoursMaxBot { get; set; }

        [BsonElement("disabled_message")]
        public string DisabledMessage { get; set; }

        [BsonElement("choices_maxbet")]
        public Dictionary<string, double> MaxBetChoices {get; set;}

        public Dictionary<string, double> Odds { get; set; }

        public int DelayStartTime { get; set; }

        public bool EnableFreeBet { get; set; }
    }
}
