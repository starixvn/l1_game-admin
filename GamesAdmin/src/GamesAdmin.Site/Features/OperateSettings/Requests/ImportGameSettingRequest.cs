using GamesAdmin.Core.Models.GameConfigs;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.OperateSettings.Requests
{
    public class ImportGameSettingRequest
    {
        public IEnumerable<GameSetting> GameSettings { get; set; }
    }

    public class GameSetting
    {
        public string Id { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public GameInfo Info { get; set; }

        public EnableSetting EnableSetting { get; set; }

        public BotSetting BotSetting { get; set; }

        public BetLimit BetLimit { get; set; }

        public SlotsConfig SlotsConfig { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }

        public SlotRNGConfig SlotRNGConfig { get; set; }

        public CrashConfig CrashConfig { get; set; }

        public IEnumerable<Dictionary<string, double>> Margin { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }
    }
}
