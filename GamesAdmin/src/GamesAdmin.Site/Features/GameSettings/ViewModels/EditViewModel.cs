using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site._Shared.Enumerations;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class EditViewModel : ViewModelBase
    { 
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) 
            ? string.Empty 
            : Enumeration.FromValue<GameType>(Name).DisplayName;

        [DisplayName("Game Enabled")]
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
        public double MaxBot { get; set; }

        public List<SelectListItem> BotPercentageItems { get; set; }        

        public int BotCount { get; set; }

        public int CurrentBot => (BotCount > 0 && MaxBot > 0) ? (int)(BotCount * MaxBot / 100) : 0;

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(0.01, 100, ErrorMessage = "Enter number between 0.01 to 100")]
        [DisplayName("Bot Minimum Bet Limit")]
        public double BotMinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(0.01, 1000, ErrorMessage = "Enter number between 0 to 1000")]
        [DisplayName("Bot Maximum Bet Limit")]
        public double BotMaxBet { get; set; }

        public double[] HoursMaxBot { get; set; }

        public List<BotRatioItemViewModel> BotRatioItems { get; set; }
    }

    public class BotRatioItemViewModel 
    {
        public BotRatioItemViewModel(int greenwichHour, int asiaHour, double botRatio, int totalBot) 
        {
            GreenwichHour = greenwichHour;
            AsiaPacificHour = asiaHour;
            BotRatio = botRatio;
            TotalBot = totalBot;
        }

        public int GreenwichHour { get; }

        public int AsiaPacificHour { get; }        

        public int TotalBot { get; }

        public int CurrentBot => (int)( BotRatio * TotalBot / 100);

        public double BotRatio { get; set; }
    }
}
