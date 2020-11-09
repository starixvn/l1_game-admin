using System.Collections.Generic;

namespace GamesAdmin.Database.Entities.GameConfigs
{
    public class SlotsGameConfig
    {
        public bool EnabledJackpot { get; set; }

        public byte Difficulty { get; set; }

        public byte JackpotRTP { get; set; }

        public float PlayerContributionRatio { get; set; }

        public float BotContributionRatio { get; set; }

        public float BigWinRatio { get; set; }

        public List<SlotsStakeLevel> StakeLevels { get; set; }

        public List<SlotIcon> Icons { get; set; }

        public Bonus Bonus { get; set; }
    }

    public class SlotsStakeLevel
    {
        public float Stake { get; set; }

        public double MinJackpot { get; set; }
    }

    public class SlotIcon
    {
        public string Name { get; set; }

        public List<SlotIconOdds> Odds { get; set; }
    }

    public class SlotIconOdds
    {
        public byte Count { get; set; }

        public float Odds { get; set; }
    }

    public class Bonus
    {
        public float Min { get; set; }

        public float Max { get; set; }
    }
}
