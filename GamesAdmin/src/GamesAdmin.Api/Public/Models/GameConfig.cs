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

        public SlotsConfigModel SlotsConfig { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set;}
    }

    public class SlotsConfigModel
    {
        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }
        public JackpotModel Jackpot { get; set; }
    }

    public class JackpotModel
    {
        public bool Enabled { get; set; }

    }
}