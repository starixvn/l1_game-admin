using System;
using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("global_configs")]
    public class GlobalConfigEntity : MiniGameBaseEntity
    {
        public GlobalConfigEntity()
        {
            Time = DateTime.UtcNow;
            Configs = new Dictionary<string, IList<string>>();
        }

        public GlobalConfigEntity(string type)
        {
            Type = type;
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