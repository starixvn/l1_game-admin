using System.Collections.Generic;
using System.ComponentModel;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class JackpotViewModel 
    {
        public bool Enabled { get; set; }

        public bool ExcludeTestCurrency { get; set; }

        [DisplayName("Winning Ratio (%)")]
        public double WinningRatio { get; set; }

        [DisplayName("Jackpot Service Fee (%)")]
        public double JackpotServiceFee => WinningRatio.ReversePercent();

        public JackpotPools Pools { get; set; }

        [DisplayName("Bot Contribution (%)")]
        public double BotContribution { get; set; }

        [DisplayName("Player Contribution (%)")]
        public double PlayerContribution { get; set; }

        [DisplayName("Allowed Currency")]
        public string AllowedCurrency { get; set; } = "INH";

        public List<StakeLevel> StakeLevels { get; set; }
    }
}