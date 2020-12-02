using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class SlotConfigExtensions
    {
        public static SlotsConfig GenerateDefaultSlotIcons(this SlotsConfig config, int total)
        {
            config.Icons = new Dictionary<string, Dictionary<string, float>>();

            for (var i = 0; i < total; i++)
            {
                config.Icons.Add(i.ToString(), GenerateDefaultOdds());
            }

            return config;
        }

        private static Dictionary<string, float> GenerateDefaultOdds()
        => new Dictionary<string, float> {
                { "2" , 0},
                { "3" , 0},
                { "4" , 0},
                { "5" , 0},
            };
    }
}