using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features._Shared;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class EditViewModel : ViewModelBase
    {
        private const int MaxPercentage = 100;
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        [DisplayName("Game Status:")]
        public bool Enabled { get; set; }

        public string StatusDisplay => Enabled ? "Enabled" : "Disabled";     

        [DisplayName("Bot Enabled")]
        public bool BotEnabled { get; set; }

        [DisplayName("Bot Max Bet Limit")]
        public double BotMaxBet { get; set; }   

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

        public SlotViewModel SlotViewModel { get; set; }

        public PayoutViewModel PayoutViewModel { get; set; }

        public JackpotViewModel JackpotViewModel { get; set; }

        [DisplayName("Enable Smart Bot")]
        public bool SmartBotEnabled { get; set; }

        [DisplayName("Smart Bot Group Ratio (%)")]
        [Range(0, MaxPercentage, ErrorMessage = "Number range [0..100]")]
        public double SmartBotGroupRatio { get; set; }
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