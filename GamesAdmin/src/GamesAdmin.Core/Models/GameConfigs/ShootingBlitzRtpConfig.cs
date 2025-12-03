using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    public class ShootingBlitzRtpConfig
    {
        public string Id { get; set; }

        public double Rtp { get; set; }

        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
    }

    public class MultiplierStepsConfig
    {
        public int Mode { get; set; }

        public string Name { get; set; }

        public double StarPlayerRate { get; set; }

        public List<int> StarPosition { get; set; }

        /// <summary>
        /// Pays array where index+1 = picks (e.g., pays[0] = picks 1, pays[1] = picks 2)
        /// </summary>
        public List<double> Pays { get; set; }
    }
}

