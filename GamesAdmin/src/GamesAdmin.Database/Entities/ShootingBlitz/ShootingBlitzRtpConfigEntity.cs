using System.Collections.Generic;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities.ShootingBlitz
{
    [BsonCollection("ShootingBlitz_rtpConfig")]
    public class ShootingBlitzRtpConfigEntity : MiniGameBaseEntity
    {
        /// <summary>
        /// RTP Config 顯示名稱 (e.g., "RTP 95% - Normal Mode", "Live Mode Config")
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("multiplierSteps")]
        public List<MultiplierStepsConfigEntity> MultiplierSteps { get; set; }
    }

    public class MultiplierStepsConfigEntity
    {
        /// <summary>
        /// 對手數量（等同於地雷數量，選到對手即輸）
        /// </summary>
        [BsonElement("opponentCount")]
        public int OpponentCount { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// 主場球員星級球員設定
        /// </summary>
        [BsonElement("home")]
        public StarPlayerSettingsEntity Home { get; set; }

        /// <summary>
        /// 客場球員星級球員設定
        /// </summary>
        [BsonElement("away")]
        public StarPlayerSettingsEntity Away { get; set; }

        /// <summary>
        /// Pays array where index+1 = picks (e.g., pays[0] = picks 1, pays[1] = picks 2)
        /// </summary>
        [BsonElement("pays")]
        public List<double> Pays { get; set; }
    }

    /// <summary>
    /// 星級球員設定（主場/客場通用）
    /// </summary>
    public class StarPlayerSettingsEntity
    {
        /// <summary>
        /// 星級球員出現率
        /// </summary>
        [BsonElement("starPlayerRate")]
        public double StarPlayerRate { get; set; }

        /// <summary>
        /// 星級球員可出現的 Pick 位置
        /// </summary>
        [BsonElement("starPositions")]
        public List<int> StarPositions { get; set; }
    }
}

