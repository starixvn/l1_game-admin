using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("global_configs")]
    public class GlobalConfigEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        public GlobalConfigEntity()
        {
            Time = DateTime.UtcNow;
            Configs = new Dictionary<string, IList<string>>();
        }

        public GlobalConfigEntity(string type)
        {
            Time = DateTime.UtcNow;
            Type = type;
            Configs = new Dictionary<string, IList<string>>();
        }

        public GlobalConfigEntity(string type, Dictionary<string, IList<string>> configs)
        {
            Time = DateTime.UtcNow;
            Type = type;
            Configs = configs;
        }

        public string Type { get; set; }

        public Dictionary<string, IList<string>> Configs { get; set; }
    }
}