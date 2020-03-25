using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class GameConfig
    {
        [JsonConstructor]
        public GameConfig(
            string id, 
            string name, 
            double minBet, 
            double maxBet, 
            bool enabled, 
            bool botEnabled,
            double maxBot,
            int? disabledround)
        {
            Id = id;
            Name = name;
            MinBet = minBet;
            MaxBet = maxBet;
            Enabled = enabled;
            BotEnabled = botEnabled;
            MaxBot = maxBot;
            DisabledRound = disabledround;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public bool BotEnabled { get; }

        public double MaxBot { get; }

        public int? DisabledRound { get; }
    }
}
