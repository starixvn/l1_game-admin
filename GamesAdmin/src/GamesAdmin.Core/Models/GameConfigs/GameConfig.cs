using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using Newtonsoft.Json;

namespace GamesAdmin.Core.Models
{
    public class GameConfig
    {
        [JsonConstructor]
        public GameConfig(
            string id,
            Dictionary<string, double> odds,
            GameInfo info = null,
            EnableSetting enableSetting = null,
            BotSetting botSettings = null,
            BetLimit betLimit = null,
            SlotsConfig slotsConfig = null,
            CascadingSlotsConfig cascadingSlotsConfig = null,
            SlotRNGConfig slotRNGConfig = null,
            CrashConfig crashConfig = null)
        {
            Id = id;
            Odds = odds;
            Info = info;
            EnableSetting = enableSetting;
            BotSetting = botSettings;
            BetLimit = betLimit;
            SlotsConfig = slotsConfig;
            CascadingSlotsConfig = cascadingSlotsConfig;
            SlotRNGConfig = slotRNGConfig;
            CrashConfig = crashConfig;
        }

        public string Id { get; }

        public Dictionary<string, double> Odds { get; }

        public GameInfo Info { get; set; }

        public EnableSetting EnableSetting { get; set; }

        public BotSetting BotSetting { get; set; }

        public BetLimit BetLimit { get; set; }

        public SlotsConfig SlotsConfig { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }

        public SlotRNGConfig SlotRNGConfig { get; set; }

        public CrashConfig CrashConfig { get; set; }
    }
}