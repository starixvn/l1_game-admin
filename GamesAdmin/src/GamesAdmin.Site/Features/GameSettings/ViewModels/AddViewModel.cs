using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddViewModel
    {
        [Required(ErrorMessage = "Name could not empty")]
        [MinLength(2)]
        [MaxLength(50)]
        [Remote(action: "VerifyName", controller: "GameSettings", AdditionalFields = nameof(Name))]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(0.01, 100, ErrorMessage = "Enter number between 0.01 to 100")]
        [DisplayName("Minimum Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(0.01, 1000, ErrorMessage = "Enter number between 0 to 1000")]
        [DisplayName("Maximum Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

      
        [DisplayName("Maximum Bot Percentage")]
        public int MaxBot { get; set; }

        public List<SelectListItem> BotPercentageItems { get; set; }

        public int BotCount { get; set; }

        public int CurrentBot => (int)(BotCount * MaxBot / 100);
    }
}
