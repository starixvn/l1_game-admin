using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Api.Public.Models
{
    public class GameConfig
    {
        public int GameId { get; set; }

        public byte Type { get; set; }

        public string Name { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public SlotsConfig SlotsConfig { get; set; }
    }
}