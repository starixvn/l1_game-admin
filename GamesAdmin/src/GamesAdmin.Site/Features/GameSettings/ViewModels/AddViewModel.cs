using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        private const double Min = 0.01;

        public AddViewModel(
            List<SelectListItem> gameTypes,
            List<SelectListItem> difficultyItems,
            List<StakeLevel> stakes
            )
        {
            GameTypes = gameTypes;
            DifficultyItems = difficultyItems;
            Stakes = stakes;
            Difficulty = SlotDifficulties.Medium;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Type")]
        public GameType GameType { get; set; }

        public List<SelectListItem> GameTypes { get; set; }

        [DisplayName("Game Enabled")]
        public bool Enabled { get; set; }

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(Min, int.MaxValue)]
        [DisplayName("Min Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(Min, int.MaxValue)]
        [DisplayName("Max Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [DisplayName("Delay Start Time")]
        public int DelayStartTime { get; set; }

        [DisplayName("Enable Free Bet")]
        public bool EnableFreeBet { get; set; }

        public SlotDifficulties Difficulty { get; set; }

        public List<SelectListItem> DifficultyItems { get; set; }

        [DisplayName("Number of Slot Icons")]
        public byte NumberOfIcons { get; set; }

        public List<StakeLevel> Stakes { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        [DisplayName("Enable Jackpot")]
        public bool EnableJackpot { get; set; }
    }
}
