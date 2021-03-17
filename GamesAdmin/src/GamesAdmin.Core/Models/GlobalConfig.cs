using System.Collections.Generic;

namespace GamesAdmin.Core.Models
{
    public class GlobalConfig
    {
        public GlobalConfig(string type, Dictionary<string, IList<string>> configs)
        {
            Type = type;
            Configs = configs;
        }

        public string Type { get; }

        public Dictionary<string, IList<string>> Configs { get; }
    }
}