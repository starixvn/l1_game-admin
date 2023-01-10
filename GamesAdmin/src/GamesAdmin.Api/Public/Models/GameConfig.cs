using System.Collections.Generic;

namespace GamesAdmin.Api.Public.Models
{
    public class GameConfig
    {
        public int GameId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public Dictionary<string, double> Odds { get; set; }
    }
}