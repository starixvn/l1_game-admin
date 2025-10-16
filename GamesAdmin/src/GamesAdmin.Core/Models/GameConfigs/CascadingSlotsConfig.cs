using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class CascadingSlotsConfig
    {
        public CascadingSlotsConfig() { }

        public CascadingSlotsConfig(byte mode, double bigWinRatio, MultiplierBonus multiplierBonus)
        {
            Mode = mode;
            BigWinRatio = bigWinRatio;
            MultiplierBonus = multiplierBonus;
        }

        public CascadingSlotsConfig(CascadingSlotsConfig config)
        {
            Mode = config.Mode;
            BigWinRatio = config.BigWinRatio;
            MegaWinRatio = config.MegaWinRatio;
            SuperWinRatio = config.SuperWinRatio;
            MultiplierBonus = config.MultiplierBonus;
            BetLevels = config.BetLevels;
            BetMultiplier = config.BetMultiplier;
            MaxWinPerRound = config.MaxWinPerRound;
            OddsByCounts = config.OddsByCounts;
            Volatility = config.Volatility;
            ResultChains = config.ResultChains;
        }

        public byte Mode { get; set; }
        public double BigWinRatio { get; set; }
        public double MegaWinRatio { get; set; }
        public double SuperWinRatio { get; set; }
        public MultiplierBonus MultiplierBonus { get; set; }
        public int[] BetLevels { get; set; }
        public double BetMultiplier { get; set; }
        public double MaxWinPerRound { get; set; }
        public Dictionary<string, List<OddsByCount>> OddsByCounts { get; set; }
        public FreeSpins FreeSpins { get; set; }

        public byte Volatility { get; set; }
        public CascadingSlotFeatures Features { get; set; }
        public ResultChains ResultChains { get; set; }
    }

    public class ResultChains
    {
        public bool Enabled { get; set; }
        public bool Reload { get; set; }
    }

    public class CascadingSlotFeatures
    {
        public BuyFreeSpinSetting BuyFreeSpins { get; set; }
    }

    public class BuyFreeSpinSetting
    {
        public BuyFreeSpinSetting()
        {
        }
        public bool Enabled { get; set; }

        public int Multiplier { get; set; }

        public BuyFreeSpinSiteExclusion[] SiteExclusions { get; set; }

    }

    public class BuyFreeSpinSiteExclusion
    {
        public string OperatorId { get; set; }

        public string[] Sites { get; set; }
    }
}
