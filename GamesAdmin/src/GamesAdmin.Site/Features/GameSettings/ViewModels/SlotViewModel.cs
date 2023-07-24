using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class SlotViewModel
    {
        public SlotViewModel() { }
        public GameMode Mode { get; set; }

        public List<SelectListItem> ModeItems { get; set; } = Enum.GetValues(typeof(GameMode))
                .Cast<GameMode>()
                .Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

        public List<StakeLevel> StakeLevels { get; set; }

        public Dictionary<string, Dictionary<string, float>> Icons { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        public bool JackpotEnabled { get; set; }

        public Dictionary<string, int[][]> SymbolWeight { get; set; }

        public byte DisplayMode { get; set; } = 1;

        public string WeightsByEasyMode => SymbolWeight?.ContainsKey("1") == true ?
            string.Join("\r\n", SymbolWeight["1"].Select(row => string.Join(",", row))) : string.Empty;

        public string WeightsByMediumMode => SymbolWeight?.ContainsKey("2") == true ?
            string.Join("\r\n", SymbolWeight["2"].Select(row => string.Join(",", row))) : string.Empty;

        public string WeightsByHardMode => SymbolWeight?.ContainsKey("3") == true ?
            string.Join("\r\n", SymbolWeight["3"].Select(row => string.Join(",", row))) : string.Empty;

        [DisplayName("Jackpot Winning Amount (%)")]
        public double JackpotWinningAmount { get; set; }

        [DisplayName("Jackpot Service Fee (%)")]
        public double JackpotServiceFee => 100 - JackpotWinningAmount;

        [DisplayName("Bot Contribution (%)")]
        public double BotJackpotContribution { get; set; }

        [DisplayName("Player Contribution (%)")]
        public double PlayerJackpotContribution { get; set; }

        [DisplayName("Big Win Ratio (%)")]
        public double BigWinRatio { get; set; }
    }
}