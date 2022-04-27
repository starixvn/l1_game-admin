using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class SlotConfigExtensions
    {
        public static SlotsConfig GenerateDefaultSlotInfo(this SlotsConfig config, int total, int maxNumber = 4)
        {
            config.Icons = new Dictionary<string, Dictionary<string, double>>();

            for (var i = 0; i < total; i++)
            {
                var iconOdds = new Dictionary<string, double>();

                for (var j = 1; j <= maxNumber; j++)
                {
                    iconOdds.Add((j + 1).ToString(), 0);
                }

                config.Icons.Add(i.ToString(), iconOdds);
            }

            return config;
        }
    }
}