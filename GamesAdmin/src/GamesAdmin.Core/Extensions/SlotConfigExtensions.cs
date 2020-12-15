using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class SlotConfigExtensions
    {
        public static SlotsConfig GenerateDefaultSlotIcons(this SlotsConfig config, int total, int maxNumber = 4)
        {
            config.Icons = new Dictionary<string, Dictionary<string, double>>();

            for (var i = 0; i < total; i++)
            {
                config.Icons.Add(i.ToString(), GenerateDefaultOdds(maxNumber));
            }

            return config;
        }

        private static Dictionary<string, double> GenerateDefaultOdds(int maxNumber)
        {
            var iconOdds = new Dictionary<string, double>();

            for (var i = 1; i <= maxNumber; i++)
            {
                iconOdds.Add((i + 1).ToString(), 0);
            }

            return iconOdds;
        }
    }
}