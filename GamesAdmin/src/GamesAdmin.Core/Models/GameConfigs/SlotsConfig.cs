using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class SlotsConfig
    {
        public SlotsConfig()
        {
        }

        public SlotsConfig(byte mode, double bigWinRatio, Bonus bonus)
        {
            Mode = mode;
            BigWinRatio = bigWinRatio;
            Bonus = bonus;
        }

        public byte Mode { get; set; }

        public double BigWinRatio { get; set; }

        public List<StakeLevel> StakeLevels { get; set; }

        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }

        public Bonus Bonus { get; set; }

        public Jackpot Jackpot { get; set; }
    }

    public class SlotIconOdds
    {
        public byte Count { get; set; }

        public float Odds { get; set; }
    }

    public class Bonus
    {
        public Bonus(double minBonus, double maxBonus)
        {
            Min = minBonus;
            Max = maxBonus;
        }

        public double Min { get; set; }

        public double Max { get; set; }
    }

    public class Jackpot
    {
        public Jackpot(bool enabled, double winingAmount, double playerContribution, double botContribution, bool excludeUUS = false)
        {
            Enabled = enabled;
            WinningRatio = winingAmount;
            PlayerContributionRatio = playerContribution;
            BotContributionRatio = botContribution;
            ExcludeUUS = excludeUUS;
        }

        public bool Enabled { get; set; }

        public bool ExcludeUUS { get; set; }

        public double WinningRatio { get; set; }

        public double PlayerContributionRatio { get; set; }

        public double BotContributionRatio { get; set; }
    }
}