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
        /// <summary>
        /// 對手數量（等同於地雷數量，選到對手即輸）
        /// </summary>
        public int OpponentCount { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 主場球員星級球員設定
        /// </summary>
        public StarPlayerSettings Home { get; set; }

        /// <summary>
        /// 客場球員星級球員設定
        /// </summary>
        public StarPlayerSettings Away { get; set; }

        /// <summary>
        /// Pays array where index+1 = picks (e.g., pays[0] = picks 1, pays[1] = picks 2)
        /// </summary>
        public List<double> Pays { get; set; }
    }

    /// <summary>
    /// 星級球員設定（主場/客場通用）
    /// </summary>
    public class StarPlayerSettings
    {
        /// <summary>
        /// 星級球員出現率
        /// </summary>
        public double StarPlayerRate { get; set; }

        /// <summary>
        /// 星級球員可出現的 Pick 位置
        /// </summary>
        public List<int> StarPositions { get; set; }
    }
}

