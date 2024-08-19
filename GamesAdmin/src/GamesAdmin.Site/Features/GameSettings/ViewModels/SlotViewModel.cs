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
        public int[][] WeightsByHardMode  => GetSymbolWeight("3");

        public Jackpot Jackpot { get; set; }

        [DisplayName("Big Win Ratio (%)")]
        public double BigWinRatio { get; set; }

        public int[][] GetSymbolWeight(string key)
        {
            if (SymbolWeight != null && SymbolWeight.TryGetValue(key, out int[][] symbolWeight))
            {
                return symbolWeight;
            }

            return Array.Empty<int[]>();
        }
    }
}