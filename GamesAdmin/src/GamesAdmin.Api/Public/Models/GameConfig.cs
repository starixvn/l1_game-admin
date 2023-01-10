using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Api.Public.Models
{
    public class GameConfig
    {
        public int GameId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public SlotsConfig SlotsConfig { get; set; }
    }
}