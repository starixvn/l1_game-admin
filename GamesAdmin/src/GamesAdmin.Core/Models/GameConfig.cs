using System;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class GameConfig
    {
        [JsonConstructor]
        public GameConfig(Guid Id, string name, double minBet, double maxBet, bool enabled, int? round = null)
        {
            this.Id = Id;
            Name = name;
            MinBet = minBet;
            MaxBet = maxBet;
            Enabled = enabled;
            Round = round;
        }

        public Guid Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public int? Round { get; }
    }
}
