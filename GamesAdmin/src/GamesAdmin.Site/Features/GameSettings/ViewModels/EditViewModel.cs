using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        private const int MinChoices = 2;
        private const double MinBetAmount = 0.01;
        private const int MaxPercentage = 100;

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name)
            ? string.Empty
            : Enumeration.FromValue<GameType>(Name).DisplayName;

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

        public double MaxBot { get; set; }

        public int BotCount { get; set; }

        [Required(ErrorMessage = "Maximum Bet could not empty")]
        [Range(MinBetAmount, int.MaxValue)]
        [DisplayName("Bot Maximum Bet Limit")]
        public double BotMaxBet { get; set; }

        public double[] HoursMaxBot { get; set; }

        public List<BotRatioItemViewModel> BotRatioItems { get; set; }

        public string DisabledMessage { get; set; }

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

        [DisplayName("Enable Smart Bot")]
        public bool SmartBotEnabled { get; set; }

        [DisplayName("Ratio Of Groups Picked By Date (%)")]
        [Range(0, MaxPercentage, ErrorMessage = "Number range [0..100]")]
        public double SmartBotTotalGroup { get; set; }
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