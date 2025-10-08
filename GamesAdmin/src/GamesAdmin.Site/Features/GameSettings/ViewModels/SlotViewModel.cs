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

        public double[] BetLevels { get; set; }

        public Dictionary<string, Dictionary<string, float>> Icons { get; set; }

        public double MinBonus { get; set; }

        public double MaxBonus { get; set; }

        public bool JackpotEnabled { get; set; }

        public Dictionary<string, int[][]> SymbolWeight { get; set; }

        public byte BoardMode { get; set; } = 1;

        public int[][] WeightsByEasyMode => GetSymbolWeight("1");
        public int[][] WeightsByMediumMode => GetSymbolWeight("2");
        public int[][] WeightsByHardMode => GetSymbolWeight("3");

        public Jackpot Jackpot { get; set; }

        [DisplayName("Big Win Ratio (%)")]
        public double BigWinRatio { get; set; }

        [DisplayName("Mega Win Ratio (%)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Mega Win Ratio must be greater than 0")]
        public double MegaWinRatio { get; set; }

        [DisplayName("Super Win Ratio (%)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Super Win Ratio must be greater than 0")]
        public double SuperWinRatio { get; set; }

        public int[][] GetSymbolWeight(string key)
        {
            if (SymbolWeight != null && SymbolWeight.TryGetValue(key, out int[][] symbolWeight))
            {
                return symbolWeight;
            }

            return Array.Empty<int[]>();
        }
        public byte Volatility { get; set; } = 2;

        private static string GetTextVolatility(byte value)
        {
            switch (value)
            {
                case 2:
                    return "Low";
                case 3:
                    return "Medium";
                case 4:
                    return "Medium High";
                case 5:
                    return "High";
                default:
                    return "";
            }
        }
        
        public List<SelectListItem> VolatilityItems = new[] { 2, 5 }
        .Select(x => new SelectListItem { Text = $"{GetTextVolatility((byte)x)} {x.ToString()} - Flashes", Value = x.ToString() })
        .ToList();

    }
}