using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Models
{
    public class JackpotConfig
    {
        public int GameId { get; set; }

        public string OperatorId { get; set; }

        public IEnumerable<string> SiteNames { get; set; }

        public bool Enabled { get; set; }

        public double WinningRatio { get; set; }

        public JackpotPools Pools { get; set; }

        public bool ExcludeTestCurrency { get; set; }
    }
}