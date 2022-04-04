using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class SlotConfigExtensions
    {
        public static SlotsConfig GenerateDefaultSlotIcons(this SlotsConfig config, GameType type, int total, int maxNumber = 4)
        {
            if (type == GameType.CascadingSlots)
            {
                GenerateDefaultCascadingSlotIcons(config, total);
            }
            else
            {
                config.Icons = new Dictionary<string, Dictionary<string, double>>();
                for (var i = 0; i < total; i++)
                {
                    config.Icons.Add(i.ToString(), GenerateDefaultOdds(maxNumber));
                }
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

        public static void GenerateDefaultCascadingSlotIcons(this SlotsConfig config, int total)
        {
            config.OddsByCounts = new Dictionary<string, List<OddsByCount>>();
            if (total == 0)
            {
                return;
            }

            var scratterIcon = total - 1;

            for (var i = 0; i < scratterIcon; i++)
            {
                config.OddsByCounts.Add(i.ToString(), new List<OddsByCount>
                {
                    new OddsByCount(true, 0, 8, 9, 0),
                    new OddsByCount(true, 0, 10, 11, 0),
                    new OddsByCount(true, 0, 12, 30, 0)
                });
            }

            config.OddsByCounts.Add(scratterIcon.ToString(), new List<OddsByCount> {
                new OddsByCount(false, 4, 0, 0, 0),
                new OddsByCount(false, 5, 0, 0, 0),
                new OddsByCount(false, 6, 0, 0, 0)
            });
        }
    }
}