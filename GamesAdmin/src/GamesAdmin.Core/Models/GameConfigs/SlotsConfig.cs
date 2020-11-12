using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class SlotsConfig
    {
        public byte Difficulty { get; set; }

        public float BigWinRatio { get; set; }

        public List<StakeLevel> StakeLevels { get; set; }

        public Dictionary<string, Dictionary<string, float>> Icons { get; set; }

        public Bonus Bonus { get; set; }

        public Jackpot Jackpot { get; set; }

        public SlotsConfig GenerateDefaultSlotIcons(int total)
        {
            Icons = new Dictionary<string, Dictionary<string, float>>();

            for (var i = 0; i < total; i++)
            {
                Icons.Add(i.ToString(), new Dictionary<string, float>());
            }

            return this;
        }
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
        public Jackpot(bool enabled, double rtp, double playerContribution, double botContribution)
        {
            Enabled = enabled;
            RTP = rtp;
            PlayerContributionRatio = playerContribution;
            BotContributionRatio = botContribution;
        }

        public bool Enabled { get; set; }

        public double RTP { get; set; }

        public double PlayerContributionRatio { get; set; }

        public double BotContributionRatio { get; set; }
    }
}