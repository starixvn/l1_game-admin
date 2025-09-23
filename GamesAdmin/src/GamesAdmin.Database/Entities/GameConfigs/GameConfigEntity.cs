using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("game_configs")]
    public class GameConfigEntity : MiniGameBaseEntity
    {
        public byte Type { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Enabled { get; set; }

        public bool Botenabled { get; set; }

        [BsonElement("minbet")]
        public double Minbet { get; set; }

        [BsonElement("maxbet")]
        public double Maxbet { get; set; }

        [BsonElement("disabledround")]
        public long? DisabledRound { get; set; }

        [BsonElement("bot_maxbet")]
        public double BotMaxBet { get; set; }

        [BsonElement("hour_maxbot")]
        public double[] HoursMaxBot { get; set; }

        [BsonElement("disabled_message")]
        public string DisabledMessage { get; set; }

        [BsonElement("choices_maxbet")]
        public Dictionary<string, double> MaxBetChoices { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, string> OddsDisplayNames { get; set; }

        [BsonElement("choices")]
        public List<Choice> Choices { get; set; }



        public int DelayStartTime { get; set; }

        public bool EnableFreeBet { get; set; }

        public SlotsConfig SlotsConfig { get; set; }

        public string[] SiteExclusions { get; set; }

        public bool SmartBotEnabled { get; set; }

        public double SmartBotGroupRatio { get; set; }

        public int GameId { get; set; }

        public byte RandomMode { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }

        public SlotRNGConfig SlotRNGConfig { get; set; }

        public CrashConfig CrashConfig { get; set; }

        public IEnumerable<Dictionary<string, double>> Margin { get; set; }

        public int MaxWinPerRound { get; set; }

        public OddsMultipliersConfig OddsMultipliers { get; set; }

        public bool ApplyMultiplier { get; set; }

        public TournamentConfig TournamentConfig { get; set; }

        public VideoStreamConfig VideoStreamConfig { get; set; }

        public bool HaveTournament { get; set; }

        public bool HaveVideoStream { get; set; }

        public bool UseMargin { get; set; }
    }
}