using System.Collections.Generic;

namespace GamesAdmin.Core.Models.GameConfigs
{
    /// <summary>
    /// ShootingBlitz 遊戲配置
    /// </summary>
    public class ShootingBlitzConfig
    {
        /// <summary>
        /// RTP 設定陣列，Game Client 透過 Key 取得對應的 RTP 設定
        /// </summary>
        public List<ShootingBlitzRtpSetting> RtpSetting { get; set; }

        /// <summary>
        /// 比分設定（用於非 Live Score 模式的隨機比分產生）
        /// </summary>
        public List<ShootingBlitzScoreSetting> ScoreSetting { get; set; }

        /// <summary>
        /// 每一 Round 能夠贏的最大金額
        /// </summary>
        public double MaxWinRound { get; set; }
    }

    /// <summary>
    /// RTP 設定項目
    /// </summary>
    public class ShootingBlitzRtpSetting
    {
        /// <summary>
        /// 模式識別碼，Game Client 傳送此 Key (e.g., "normal", "live")
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// RTP Config 的 ObjectId，參考 ShootingBlitz_rtpConfig collection
        /// </summary>
        public string RtpConfigId { get; set; }
    }

    /// <summary>
    /// 比分設定（用於非 Live Score 模式）
    /// </summary>
    public class ShootingBlitzScoreSetting
    {
        /// <summary>
        /// 主場比分
        /// </summary>
        public int HomeScore { get; set; }

        /// <summary>
        /// 客場比分
        /// </summary>
        public int AwayScore { get; set; }

        /// <summary>
        /// 此比分組合的選擇機率（所有機率總和應為 1）
        /// </summary>
        public double Probability { get; set; }
    }
}

