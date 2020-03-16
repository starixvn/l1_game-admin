using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class Game
    {
        [JsonConstructor]
        public Game(string Id, string name, double minBet, double maxBet, bool enabled)
        {
            this.Id = Id;
            Name = name;
            MinBet = minBet;
            MaxBet = maxBet;
            Enabled = enabled;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }
    }
}
