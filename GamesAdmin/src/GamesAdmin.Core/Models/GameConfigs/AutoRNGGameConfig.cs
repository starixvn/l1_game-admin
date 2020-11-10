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
            byte type,
            string name,
            string displayName,
            double minBet,
            double maxBet,
            bool enabled,
            bool botEnabled,
            int? disabledround,
            double botMaxBet,
            double[] hoursMaxBot,
            string disabledMessage,
            Dictionary<string, double> maxBetChoices,
            Dictionary<string, double> odds,
            int delayStartTime,
            bool enableFreeBet)
            : base(
               id,
               type,
               name,
               displayName,
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
            MaxBetChoices = maxBetChoices;
            Odds = odds;
        }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetChoices { get; }

        public Dictionary<string, double> Odds { get; }
    }
}
