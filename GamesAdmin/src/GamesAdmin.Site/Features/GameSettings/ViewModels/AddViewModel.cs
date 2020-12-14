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
        private readonly double[] DefaultBotRatios = new double[] { 40, 40, 20 };

        public AddViewModel()
        {
        }

        public AddViewModel(List<SelectListItem> gameTypes, List<SelectListItem> difficultyItems)
        {
            GameTypes = gameTypes;
            ModeItems = difficultyItems;
            Mode = GameMode.Medium;
            Stakes = new double[] { 0, 0, 0 };
            Jackpots = new double[] { 0, 0, 0 };
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Type")]
        public GameType Type { get; set; }

        public List<SelectListItem> GameTypes { get; set; }

        [DisplayName("Game Enabled")]
        public bool Enabled { get; set; }

        [DisplayName("Min Bet Limit")]
        public double MinBet { get; set; }

        [DisplayName("Max Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [DisplayName("Delay Start Time")]
        public int DelayStartTime { get; set; }

        [DisplayName("Enable Free Bet")]
        public bool EnableFreeBet { get; set; }

        public GameMode Mode { get; set; }

        public List<SelectListItem> ModeItems { get; set; }

        [DisplayName("Number of Slot Icons")]
        public byte NumberOfIcons { get; set; }

        public double[] Stakes { get; set; }

        public double[] Jackpots { get; set; }

        public double[] BotRatios => DefaultBotRatios;

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        [DisplayName("Enable Jackpot")]
        public bool EnableJackpot { get; set; }

        [DisplayName("Big Win Ratio")]
        public double BigWinRatio { get; set; }

        [DisplayName("RTP")]
        public double JackpotRTP { get; set; }

        [DisplayName("Bot Contribution")]
        public double BotJackpotContribution { get; set; }

        [DisplayName("Player Contribution")]
        public double PlayerJackpotContribution { get; set; }
    }
}