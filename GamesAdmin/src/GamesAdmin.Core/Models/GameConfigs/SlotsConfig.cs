using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class SlotsConfig
    {
        public SlotsConfig()
        {
        }

        public SlotsConfig(byte mode, double bigWinRatio, Bonus bonus, double multiplier = 0)
        {
            Mode = mode;
            BigWinRatio = bigWinRatio;
            Bonus = bonus;
            Multiplier = multiplier;
        }

        public SlotsConfig(SlotsConfig config)
        {
            Mode = config.Mode;
            BigWinRatio = config.BigWinRatio;
            Bonus = config.Bonus;
        }

        public byte Mode { get; set; }
        public double BigWinRatio { get; set; }
        public Bonus Bonus { get; set; }

        public List<StakeLevel> StakeLevels { get; set; }
        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }
        public Jackpot Jackpot { get; set; }

        public int[] BetLevels { get; set; }
        public double[] CoinValues { get; set; }
        public double Multiplier { get; set; }
        public double MaxWinPerRound { get; set; }
        public Dictionary<string, List<OddsByCount>> OddsByCounts { get; set; }

        public SlotsConfig StandardizeSlotsConfig(GameType type)
        {
            var result = new SlotsConfig(this);

            if (type == GameType.CascadingSlots)
            {
                result.BetLevels = BetLevels;
                result.CoinValues = CoinValues;
                result.Multiplier = Multiplier;
                result.MaxWinPerRound = MaxWinPerRound;
                result.OddsByCounts = OddsByCounts;
            }
            else
            {
                result.StakeLevels = StakeLevels;
                result.Icons = Icons;
                result.Jackpot = Jackpot;
            }

            return result;
        }
    }

    public class OddsByCount
    {
        public OddsByCount()
        {
        }

        public OddsByCount(bool isRange, int count, int from, int to, double odds)
        {
            IsRange = isRange;
            Count = count;
            From = from;
            To = to;
            Odds = odds;
        }

        public bool IsRange { get; set; }

        public int Count { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public double Odds { get; set; }
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
        public Jackpot(
            bool enabled,
            double winingAmount,
            double playerContribution,
            double botContribution,
            bool excludeTestCurrency = false,
            string allowedCurrency = "INH")
        {
            Enabled = enabled;
            WinningRatio = winingAmount;
            PlayerContributionRatio = playerContribution;
            BotContributionRatio = botContribution;
            ExcludeTestCurrency = excludeTestCurrency;
            AllowedCurrency = (allowedCurrency ?? string.Empty).Trim();
        }

        public bool Enabled { get; set; }

        public bool ExcludeTestCurrency { get; set; }

        public double WinningRatio { get; set; }

        public double PlayerContributionRatio { get; set; }

        public double BotContributionRatio { get; set; }

        public string AllowedCurrency { get; set; }
    }
}