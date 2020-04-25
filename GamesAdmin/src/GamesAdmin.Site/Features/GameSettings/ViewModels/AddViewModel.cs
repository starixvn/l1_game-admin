using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        [Required]
        public string Name { get; set; }

        [DisplayName("Game Enabled")]
        public bool Enabled { get; set; }

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(0.01, int.MaxValue)]
        [DisplayName("Minimum Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(0.01, int.MaxValue)]
        [DisplayName("Maximum Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

      
        [DisplayName("Maximum Bot Percentage")]
        public int MaxBot { get; set; }

        public List<SelectListItem> BotPercentageItems { get; set; }

        public List<SelectListItem> GameTypeItems { get; set; }
    }
}
