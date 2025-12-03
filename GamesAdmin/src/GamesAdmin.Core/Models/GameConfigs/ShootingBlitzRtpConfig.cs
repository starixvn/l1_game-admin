using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class ShootingBlitzRtpConfig
    {
        public string Id { get; set; }

        public double Rtp { get; set; }

        public List<BombCountConfig> BombCount { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class BombCountConfig
    {
        public int BombCount { get; set; }

        public double Probability { get; set; }
    }

    public class MultiplierStepsConfig
    {
        public int Mode { get; set; }

        public string Name { get; set; }

        public double StarPlayerRate { get; set; }

        public List<int> StarPosition { get; set; }

        public List<MultiplierItem> Multiplier { get; set; }
    }

    public class MultiplierItem
    {
        public int Picks { get; set; }

        public double Pays { get; set; }
    }
}

