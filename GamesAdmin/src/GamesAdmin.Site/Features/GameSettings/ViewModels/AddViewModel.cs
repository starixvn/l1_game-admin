using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.GameSettings.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        private readonly double[] DefaultBotRatios = new double[] { 40, 40, 20 };

        public AddViewModel() { }

        public AddViewModel(
            List<SelectListItem> gameNames,
            List<SelectListItem> gameTypes,
            List<SelectListItem> difficultyItems)
        {
            GameNames = gameNames;
            GameTypes = gameTypes;
            ModeItems = difficultyItems;
            Mode = GameMode.Medium;
            Type = GameType.AutoRNGIndex; // Set default type to AutoRNG
            Stakes = new double[] { 0, 0, 0 };
            Jackpots = new double[] { 0, 0, 0 };

            BetLevels = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string DisplayName { get; set; }

        [DisplayName("Game ID")]
        [GameIdRequired]
        public int? GameId { get; set; }

        public List<SelectListItem> GameNames { get; set; }

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

        [DisplayName("Max number of odds per Icon")]
        public byte OddsPerIcon { get; set; }

        public double[] Stakes { get; set; }

        public double[] Jackpots { get; set; }

        public double[] BotRatios => DefaultBotRatios;

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        [DisplayName("Enable Jackpot")]
        public bool EnableJackpot { get; set; }

        [DisplayName("Big Win Ratio (%)")]
        public double BigWinRatio { get; set; }

        [DisplayName("Mega Win Ratio (%)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Mega Win Ratio must be greater than 0")]
        public double MegaWinRatio { get; set; }

        [DisplayName("Super Win Ratio (%)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Super Win Ratio must be greater than 0")]
        public double SuperWinRatio { get; set; }

        [DisplayName("Winning Ratio (%)")]
        public double WinningRatio { get; set; }

        [DisplayName("Bot Contribution (%)")]
        public double BotJackpotContribution { get; set; }

        [DisplayName("Player Contribution (%)")]
        public double PlayerJackpotContribution { get; set; }

        [DisplayName("Allowed Currency")]
        public string AllowedCurrency { get; set; } = "USD";

        public int[] BetLevels { get; set; }
        public double BetMultiplier { get; set; }
        public double MaxWinPerRound { get; set; }
        public double MinMultiplierBonus { get; set; }
        public double MaxMultiplierBonus { get; set; }

        [DisplayName("Apply Multiplier")]
        public bool ApplyMultiplier { get; set; }

        [DisplayName("Have Tournament")]
        public bool HaveTournament { get; set; }

        [DisplayName("Have Video Stream")]
        public bool HaveVideoStream { get; set; }

        [DisplayName("Use Margin")]
        public bool UseMargin { get; set; }

        // Choice-related properties for manual input
        public List<ChoiceViewModel> Choices { get; set; } = new List<ChoiceViewModel>();

        // Odds settings properties
        public bool UseOddsDifferentWithChoice { get; set; }

        // For separate odds configuration form binding

        public string[] OddsKeys { get; set; } = new string[0];
        public string[] OddsDisplayNames { get; set; } = new string[0];
        public double[] OddsValues { get; set; } = new double[0];

        // Error handling properties
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }

        public void SetErrorProperties(GameCreationResult result)
        {
            HasError = true;
            ErrorMessage = result.ErrorMessage ?? "Failed to create the game. Please check your input and try again.";
            ErrorCode = result.ErrorCode;
        }

        public void CopyUserInputFrom(AddViewModel source)
        {
            // Copy basic user input
            Name = source.Name;
            DisplayName = source.DisplayName;
            GameId = source.GameId;
            Type = source.Type;
            Enabled = source.Enabled;
            DelayStartTime = source.DelayStartTime;
            MaxBet = source.MaxBet;
            MinBet = source.MinBet;
            BotEnabled = source.BotEnabled;
            
            // Copy error information
            HasError = source.HasError;
            ErrorMessage = source.ErrorMessage;
            ErrorCode = source.ErrorCode;
        }

        public void UpdateDropdownSelections()
        {
            // Set selected name in dropdown
            if (!string.IsNullOrEmpty(Name) && GameNames != null)
            {
                foreach (var item in GameNames)
                {
                    item.Selected = item.Value == Name;
                }
            }
            
            // Set selected type in dropdown
            if (GameTypes != null)
            {
                var typeValue = ((int)Type).ToString();
                foreach (var item in GameTypes)
                {
                    item.Selected = item.Value == typeValue;
                }
            }
        }
    }
}