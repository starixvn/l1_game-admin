using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class AutoRNGGameConfig : BaseConfig
    {
        [JsonConstructor]
        public AutoRNGGameConfig(
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
            double[] hoursMaxBot,
            string disabledMessage,
            Dictionary<string, double> maxBetChoices,
            Dictionary<string, double> odds,
            int delayStartTime,
            bool enableFreeBet)
            : base(
               id,
               name,
               enabled,
               botEnabled,
               disabledround,
               botMaxBet,
               hoursMaxBot,
               disabledMessage,
               delayStartTime,
               enableFreeBet)
        {

            MinBet = minBet;
            MaxBet = maxBet;
            MaxBot = maxBot;
            MaxBetChoices = maxBetChoices;
            Odds = odds;
        }

        public double MinBet { get; }

        public double MaxBet { get; }
        public double MaxBot { get; }

        public double BotMinBet { get; }

        public Dictionary<string, double> MaxBetChoices { get; }

        public Dictionary<string, double> Odds { get; }
    }
}
