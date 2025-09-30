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
            CrashConfig crashConfig = null,
            OddsMultipliersConfig oddsMultipliers = null,
            TournamentConfig tournamentConfig = null,
            VideoStreamConfig videoStreamConfig = null,
            PlinkoConfig plinkoConfig = null,
            Dictionary<string, string> oddsDisplayNames = null,
            List<Choice> choices = null)
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
            OddsMultipliers = oddsMultipliers;
            TournamentConfig = tournamentConfig;
            VideoStreamConfig = videoStreamConfig;
            OddsDisplayNames = oddsDisplayNames ?? new Dictionary<string, string>();
            Choices = choices ?? new List<Choice>();
            PlinkoConfig = plinkoConfig ?? new PlinkoConfig();
        }

        public string Id { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, string> OddsDisplayNames { get; set; }

        public List<Choice> Choices { get; set; }



        public GameInfo Info { get; set; }

        public EnableSetting EnableSetting { get; set; }

        public BotSetting BotSetting { get; set; }

        public BetLimit BetLimit { get; set; }

        public SlotsConfig SlotsConfig { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }

        public SlotRNGConfig SlotRNGConfig { get; set; }

        public CrashConfig CrashConfig { get; set; }

        public OddsMultipliersConfig OddsMultipliers { get; set; }

        public TournamentConfig TournamentConfig { get; set; }

        public VideoStreamConfig VideoStreamConfig { get; set; }

        public IEnumerable<Dictionary<string, double>> Margin { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public PlinkoConfig PlinkoConfig { get; set; }
    }

}