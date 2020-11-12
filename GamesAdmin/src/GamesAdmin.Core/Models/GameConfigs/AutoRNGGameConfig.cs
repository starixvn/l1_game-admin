using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class AutoRNGGameConfig
    {
        [JsonConstructor]
        public AutoRNGGameConfig(
            string id,
            Dictionary<string, double> odds,
            GameInfo info = null,
            EnableSetting enableSetting = null,
            BotSetting botSettings = null,
            BetLimit betLimit = null)
        {
            Id = id;
            Info = info;
            EnableSetting = enableSetting;
            BotSetting = botSettings;
            BetLimit = betLimit;
            Odds = odds;
        }

        public string Id { get; }

        public GameInfo Info { get; set; }

        public EnableSetting EnableSetting { get; set; }

        public BotSetting BotSetting { get; set; }

        public BetLimit BetLimit { get; set; }

        public Dictionary<string, double> Odds { get; }
    }
}