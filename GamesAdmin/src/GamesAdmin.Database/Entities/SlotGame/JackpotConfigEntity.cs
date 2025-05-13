using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("jackpot_configs")]
    public class JackpotConfigEntity : Document
    {
        public int GameId { get; set; }

        public string OperatorId { get; set; }

        public string[] SiteNames { get; set; }

        public bool Enabled { get; set; }

        public double WinningRatio { get; set; }

        public JackpotPools Pools { get; set; }

        public bool ExcludeTestCurrency { get; set; }

        public double[] BotBetLevels { get; set; }
    }
}