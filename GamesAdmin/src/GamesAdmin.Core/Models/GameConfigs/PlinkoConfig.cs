using System;
using System.Collections.Generic;
using System.Text;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class PlinkoConfig
    {
        public Dictionary<string, Dictionary<string, double[]>> Odds { get; set; }

        public PlinkoConfig()
        {
            Odds = InitializeDefaultOdds();
        }

        private static Dictionary<string, Dictionary<string, double[]>> InitializeDefaultOdds()
        {
            var odds = new Dictionary<string, Dictionary<string, double[]>>();
            var riskLevels = new[] { "0", "1", "2" };
            var rowCounts = new[] { 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            foreach (var riskLevel in riskLevels)
            {
                odds[riskLevel] = new Dictionary<string, double[]>();
                
                foreach (var rowCount in rowCounts)
                {
                    odds[riskLevel][rowCount.ToString()] = GenerateDefaultArray(rowCount);
                }
            }

            return odds;
        }

        private static double[] GenerateDefaultArray(int length)
        {
            return new double[length + 1];
        }
    }
}
