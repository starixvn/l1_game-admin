using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        private const int MinChoices = 2;
        private const double MinBetAmount = 0.01;

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name)
            ? string.Empty
            : Enumeration.FromValue<Game>(Name).DisplayName;

        [DisplayName("Game Status:")]
        public bool Enabled { get; set; }

        public string StatusDisplay => Enabled ? "Enabled" : "Disabled";

        [Required(ErrorMessage = "Mininum Bet could not empty")]
        [Range(MinBetAmount, int.MaxValue)]
        [DisplayName("Minimum Bet Limit")]
        public double MinBet { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(MinBetAmount, int.MaxValue)]
        [DisplayName("Maximum Bet Limit")]
        public double MaxBet { get; set; }

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(MinBetAmount, int.MaxValue)]
        [DisplayName("Bot Maximum Bet Limit")]
        public double BotMaxBet { get; set; }

        public List<MaxBetChoiceViewModel> BetChoiceBetSettings { get; set; }

        public bool DisplayMaxBetChoices => MaxBetChoices?.Count > MinChoices;

        public Dictionary<string, double> MaxBetChoices
            => BetChoiceBetSettings?.ToDictionary(x => x.Name, x => x.MaxBet);

        public List<OddsViewModel> BetChoiceOdds { get; set; }

        public Dictionary<string, double> Odds
            => BetChoiceOdds?.ToDictionary(x => x.Name, x => x.Odds);

        [DisplayName("Delay Start Time")]
        public int DelayStartTime { get; set; }

        [DisplayName("Enable Free Bet")]
        public bool EnableFreeBet { get; set; }

        public GameType Type { get; set; }

        public string TypeDisplay => Type == 0 ? Enum.GetName(typeof(GameType), GameType.AutoRNG) : Enum.GetName(typeof(GameType), Type);

        public string DisabledMessage { get; set; }

        public int BotCount { get; set; }

        public double[] HoursMaxBot { get; set; }

        public List<BotRatioItemViewModel> BotRatioItems { get; set; }

        public SlotsConfig SlotsConfig { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        [DisplayName("RTP")]
        public double JackpotRTP { get; set; }

        [DisplayName("Bot Contribution")]
        public double BotJackpotContribution { get; set; }

        [DisplayName("Player Contribution")]
        public double PlayerJackpotContribution { get; set; }

        public GameMode Mode { get; set; }

        public List<SelectListItem> ModeItems { get; set; }

        [DisplayName("Big Win Ratio")]
        public double BigWinRatio { get; set; }
    }

    public class BotRatioItemViewModel
    {
        private const int MaxPercentage = 100;

        public BotRatioItemViewModel()
        {
        }

        public BotRatioItemViewModel(int utcHour, int greenwichHour, int asiaHour, double botRatio, int totalBot)
        {
            UtcHour = utcHour;
            GreenwichHour = greenwichHour;
            AsiaPacificHour = asiaHour;
            BotRatio = botRatio;
            TotalBot = totalBot;
        }

        public int UtcHour { get; set; }

        public int GreenwichHour { get; }

        public int AsiaPacificHour { get; }

        public int TotalBot { get; }

        public int CurrentBot => (int)(BotRatio * TotalBot / MaxPercentage);

        [Range(0, MaxPercentage, ErrorMessage = "Number range [0..100]")]
        public double BotRatio { get; set; }
    }
}