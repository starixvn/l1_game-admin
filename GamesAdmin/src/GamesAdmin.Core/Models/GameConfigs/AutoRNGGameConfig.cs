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
            GameInfo info,
            EnableSettings enableSettings,
            BotSettings botSettings,
            BetLimit betLimit,
            Dictionary<string, double> odds)
            : base(
               id,
               info,
               enableSettings,
               botSettings)
        {
            MinBet = betLimit.MinBet;
            MaxBet = betLimit.MaxBet;
            MaxBetChoices = betLimit.MaxBetChoices;
            Odds = odds;
        }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetChoices { get; }

        public Dictionary<string, double> Odds { get; }
    }

    public class BetLimit
    {
        public BetLimit()
        {
        }

        public BetLimit(double min, double max, Dictionary<string, double> maxBetChoices)
        {
            MinBet = min;
            MaxBet = max;
            MaxBetChoices = maxBetChoices;
        }

        public double MinBet { get; }

        public double MaxBet { get; }

        public Dictionary<string, double> MaxBetChoices { get; }
    }
}