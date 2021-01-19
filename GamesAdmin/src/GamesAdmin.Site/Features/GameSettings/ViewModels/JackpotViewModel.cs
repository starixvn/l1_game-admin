using System.Collections.Generic;
using System.ComponentModel;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Extensions;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class JackpotViewModel
    {
        public List<StakeLevel> StakeLevels { get; set; }

        public bool JackpotEnabled { get; set; }

        [DisplayName("Exclude UUS Contribution")]
        public bool ExcludeUUS { get; set; }

        [DisplayName("Jackpot Winning (%)")]
        public double WinningRatio { get; set; }

        [DisplayName("Jackpot Service Fee (%)")]
        public double JackpotServiceFee => WinningRatio.ReversePercent();

        [DisplayName("Bot Contribution (%)")]
        public double BotContribution { get; set; }

        [DisplayName("Player Contribution (%)")]
        public double PlayerContribution { get; set; }
    }
}