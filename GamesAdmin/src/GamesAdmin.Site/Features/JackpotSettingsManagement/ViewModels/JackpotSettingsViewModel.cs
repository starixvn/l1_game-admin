using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Extensions;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels
{
    public class JackpotSettingsViewModel
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }

        [DisplayName("Game"), Required]
        public int GameId { get; set; }

        [DisplayName("Operator"), Required]
        public string OperatorId { get; set; }

        [DisplayName("Site Name")]
        public string SiteNames { get; set; }

        [DisplayName("Exclude Test Currency")]
        public bool ExcludeTestCurrency { get; set; }

        public double WinningRatio { get; set; }

        [DisplayName("Winning Ratio (%)"), Required]
        public double DisplayWinningRatio
        {
            get
            {
                return WinningRatio.ToPercentage();
            }
            set
            {
                WinningRatio = value.ConvertPercentValue();
            }
        }

        [DisplayName("Jackpot Service Fee (%)")]
        public double JackpotServiceFee => DisplayWinningRatio.ReversePercent();

        public JackpotPools Pools { get; set; }

        public List<double> BotBetLevels { get; set; } = new List<double> { 0 };

        public IEnumerable<SelectListItem> Games { get; set; }

        public IEnumerable<SelectListItem> Operators { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        public int TotalBot { get; set; }

        public List<BotRatioItemViewModel> BotRatios { get; set; }
    }
}