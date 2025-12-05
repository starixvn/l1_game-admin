using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    /// <summary>
    /// ShootingBlitz 操作類型
    /// </summary>
    public enum ShootingBlitzActionType
    {
        /// <summary>
        /// 更新遊戲配置（rtpSetting, scoreSetting, maxWinRound）
        /// </summary>
        UpdateConfig = 0,

        /// <summary>
        /// 查詢所有 RTP 配置
        /// </summary>
        QueryRtpConfigs = 1,

        /// <summary>
        /// 創建 RTP 配置
        /// </summary>
        CreateRtpConfig = 2,

        /// <summary>
        /// 更新 RTP 配置
        /// </summary>
        UpdateRtpConfig = 3,

        /// <summary>
        /// 刪除 RTP 配置
        /// </summary>
        DeleteRtpConfig = 4,

        /// <summary>
        /// 查詢所有 Teams
        /// </summary>
        QueryTeams = 5,

        /// <summary>
        /// 創建 Team
        /// </summary>
        CreateTeam = 6,

        /// <summary>
        /// 更新 Team
        /// </summary>
        UpdateTeam = 7,

        /// <summary>
        /// 刪除 Team
        /// </summary>
        DeleteTeam = 8
    }

    /// <summary>
    /// ShootingBlitz 統一配置請求
    /// </summary>
    public class UpdateShootingBlitzConfigRequest : IRequest<ShootingBlitzConfigResult>
    {
        /// <summary>
        /// 操作類型
        /// </summary>
        public ShootingBlitzActionType ActionType { get; set; }

        /// <summary>
        /// 遊戲名稱（用於 UpdateConfig, SelectRtpConfig）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// RTP 配置 ID（用於 UpdateRtpConfig, DeleteRtpConfig, SelectRtpConfig）
        /// </summary>
        public string RtpConfigId { get; set; }

        #region UpdateConfig 相關屬性

        public ShootingBlitzConfig ShootingBlitzConfig { get; set; }

        public GameInfo GameInfo { get; set; }

        public BotSetting BotSetting { get; set; }

        public EnableSetting EnableSetting { get; set; }

        #endregion

        #region RTP Config 相關屬性

        /// <summary>
        /// RTP Config 顯示名稱（用於 CreateRtpConfig, UpdateRtpConfig）
        /// e.g., "RTP 95% - Normal Mode", "Live Mode Config"
        /// </summary>
        public string RtpConfigName { get; set; }

        /// <summary>
        /// 倍率步驟配置（用於 CreateRtpConfig, UpdateRtpConfig）
        /// </summary>
        public List<MultiplierStepsConfig> MultiplierSteps { get; set; }

        #endregion
    }

    /// <summary>
    /// ShootingBlitz 配置結果
    /// </summary>
    public class ShootingBlitzConfigResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// 單一 RTP 配置結果（用於 CreateRtpConfig, UpdateRtpConfig）
        /// </summary>
        public ShootingBlitzRtpConfig RtpConfig { get; set; }

        /// <summary>
        /// RTP 配置列表（用於 QueryRtpConfigs）
        /// </summary>
        public IList<ShootingBlitzRtpConfig> RtpConfigs { get; set; }

        public static ShootingBlitzConfigResult Ok(string message = null)
            => new ShootingBlitzConfigResult { Success = true, Message = message };

        public static ShootingBlitzConfigResult Ok(ShootingBlitzRtpConfig rtpConfig)
            => new ShootingBlitzConfigResult { Success = true, RtpConfig = rtpConfig };

        public static ShootingBlitzConfigResult Ok(IList<ShootingBlitzRtpConfig> rtpConfigs)
            => new ShootingBlitzConfigResult { Success = true, RtpConfigs = rtpConfigs };

        public static ShootingBlitzConfigResult Fail(string message)
            => new ShootingBlitzConfigResult { Success = false, Message = message };
    }
}
