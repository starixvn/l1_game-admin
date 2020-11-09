using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        public AddViewModel(List<SelectListItem> games, List<SelectListItem> gameTypes) 
        {
            Games = games;
            GameTypes = gameTypes;
        }

        private const double Min = 0.01;

        [Required]
        public string Name { get; set; }

        public List<SelectListItem> Games { get; set; }

        [DisplayName("Type")]
        public GameType GameType { get; set; }

        public List<SelectListItem> GameTypes { get; set; }

        [DisplayName("Game Enabled")]
        public bool Enabled { get; set; }

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(Min, int.MaxValue)]
        [DisplayName("Minimum Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(Min, int.MaxValue)]
        [DisplayName("Maximum Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [DisplayName("Delay Start Time")]
        public int DelayStartTime { get; set; }

        [DisplayName("Enable Free Bet")]
        public bool EnableFreeBet { get; set; }
    }
}
