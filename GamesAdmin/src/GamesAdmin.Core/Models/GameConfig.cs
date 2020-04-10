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
            int? disabledround,
            double botMinBet,
            double botMaxBet,
            double[] hoursMaxBot)
        {
            Id = id;
            Name = name;
            MinBet = minBet;
            MaxBet = maxBet;
            Enabled = enabled;
            BotEnabled = botEnabled;
            MaxBot = maxBot;
            DisabledRound = disabledround;
            BotMinBet = botMinBet;
            BotMaxBet = botMaxBet;
            HoursMaxBot = hoursMaxBot;
        }

        public string Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public double MinBet { get; }

        public double MaxBet { get; }

        public bool BotEnabled { get; }

        public double MaxBot { get; }

        public int? DisabledRound { get; }

        public double BotMinBet { get; }

        public double BotMaxBet { get; }

        public double[] HoursMaxBot { get; set; }
    }
}
