using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class SlotConfigExtensions
    {
        public static SlotsConfig GenerateDefaultSlotInfo(this SlotsConfig config, GameType type, int total, int maxNumber = 4)
        {
            if (type == GameType.CascadingSlots)
            {
                GenerateDefaultCascadingSlotIcons(config, total);
            }
            else
            {
                GenerateDefaultSlotIcons(config, total, maxNumber);
            }

            return config;
        }

        private static void GenerateDefaultSlotIcons(this SlotsConfig config, int total, int maxNumber)
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
        }

        public static void GenerateDefaultCascadingSlotIcons(this SlotsConfig config, int total)
        {
            var initialFreespinsOdds = new List<FreeSpinsOdds>
            {
                new FreeSpinsOdds(4, 0),
                new FreeSpinsOdds(5, 0),
                new FreeSpinsOdds(6, 0)
            };
            var additionalFreespinsOdds = new List<FreeSpinsOdds> { new FreeSpinsOdds(5, 0) };
            config.Freespins = new Freespins(initialFreespinsOdds, additionalFreespinsOdds);

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
                    new OddsByCount(8, 9, 0),
                    new OddsByCount(10, 11, 0),
                    new OddsByCount(12, 30, 0)
                });
            }
        }
    }
}