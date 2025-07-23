using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Core.Extensions
{
    public static class CascadingSlotsConfigExtensions
    {
        public static CascadingSlotsConfig GenerateDefaultCascadingSlotsConfig(this CascadingSlotsConfig config, int total)
        {
            var initialFreeSpinsOdds = new List<FreeSpinsOdds>
            {
                new FreeSpinsOdds(4, 0),
                new FreeSpinsOdds(5, 0),
                new FreeSpinsOdds(6, 0)
            };

            var additionalFreeSpinsOdds = new List<FreeSpinsOdds> { new FreeSpinsOdds(5, 0) };
            config.FreeSpins = new FreeSpins(initialFreeSpinsOdds, additionalFreeSpinsOdds);

            config.OddsByCounts = new Dictionary<string, List<OddsByCount>>();
            if (total > 0)
            {
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

                config.OddsByCounts.Add("9", new List<OddsByCount>
                {
                    new OddsByCount(4,4,0),
                    new OddsByCount(5,5,0),
                    new OddsByCount(6,30,0),
                });

            }

            return config;
        }
    }
}