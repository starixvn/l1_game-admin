using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("global_configs")]
    public class GlobalConfigEntity : MiniGameBaseEntity
    {
        public string Type { get; set; }

        public Dictionary<string, IList<string>> Configs { get; set; }
    }
}