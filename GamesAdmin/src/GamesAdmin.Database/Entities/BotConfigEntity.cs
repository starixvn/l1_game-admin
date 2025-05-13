using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("bot_configs")]
    public class BotConfigEntity : Document
    {
        public ObjectId JackpotConfigId { get; set; }
        public int GameId { get; set; }

        public bool Enabled { get; set; }

        [BsonElement("hour_maxbot")]
        public double[] HoursMaxBot { get; set; }
    }
}