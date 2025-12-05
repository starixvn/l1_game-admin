using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;

namespace GamesAdmin.Api.GameSettings
{
    /// <summary>
    /// ShootingBlitz 統一配置服務
    /// 封裝所有 ShootingBlitz 相關邏輯，包括 RTP Config 和 Team 操作
    /// </summary>
    public interface IShootingBlitzConfigService
    {
        /// <summary>
        /// ShootingBlitz 統一配置方法（根據 ActionType 執行不同操作）
        /// </summary>
        Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig(UpdateShootingBlitzConfigRequest request);
    }

    public class ShootingBlitzConfigService : IShootingBlitzConfigService
    {
        private readonly IGameSettingsService gameService;
        private readonly IShootingBlitzRtpConfigService shootingBlitzRtpConfigService;
        private readonly IShootingBlitzTeamService shootingBlitzTeamService;

        public ShootingBlitzConfigService(
            IGameSettingsService gameService,
            IShootingBlitzRtpConfigService shootingBlitzRtpConfigService,
            IShootingBlitzTeamService shootingBlitzTeamService)
        {
            this.gameService = gameService;
            this.shootingBlitzRtpConfigService = shootingBlitzRtpConfigService;
            this.shootingBlitzTeamService = shootingBlitzTeamService;
        }

        public async Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig(UpdateShootingBlitzConfigRequest request)
        {
            switch (request.ActionType)
            {
                case ShootingBlitzActionType.UpdateConfig:
                    var updateResult = await gameService.UpdateShootingBlitzConfig(
                        request.GameInfo, request.BotSetting, request.EnableSetting, request.ShootingBlitzConfig);
                    return updateResult 
                        ? ShootingBlitzConfigResult.Ok("Config updated successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to update config");

                case ShootingBlitzActionType.QueryRtpConfigs:
                    var rtpConfigs = await shootingBlitzRtpConfigService.GetAllAsync();
                    return ShootingBlitzConfigResult.Ok(rtpConfigs as IList<ShootingBlitzRtpConfig> ?? new List<ShootingBlitzRtpConfig>(rtpConfigs));

                case ShootingBlitzActionType.CreateRtpConfig:
                    var newConfig = new ShootingBlitzRtpConfig
                    {
                        Name = request.RtpConfigName,
                        MultiplierSteps = request.MultiplierSteps
                    };
                    var createdConfig = await shootingBlitzRtpConfigService.CreateAsync(newConfig);
                    return createdConfig != null 
                        ? ShootingBlitzConfigResult.Ok(createdConfig) 
                        : ShootingBlitzConfigResult.Fail("Failed to create RTP config");

                case ShootingBlitzActionType.UpdateRtpConfig:
                    var configToUpdate = new ShootingBlitzRtpConfig
                    {
                        Name = request.RtpConfigName,
                        MultiplierSteps = request.MultiplierSteps
                    };
                    var updateRtpResult = await shootingBlitzRtpConfigService.UpdateAsync(request.RtpConfigId, configToUpdate);
                    return updateRtpResult 
                        ? ShootingBlitzConfigResult.Ok("RTP config updated successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to update RTP config");

                case ShootingBlitzActionType.DeleteRtpConfig:
                    var deleteResult = await shootingBlitzRtpConfigService.DeleteAsync(request.RtpConfigId);
                    return deleteResult 
                        ? ShootingBlitzConfigResult.Ok("RTP config deleted successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to delete RTP config");

                case ShootingBlitzActionType.SelectRtpConfig:
                    var selectResult = await gameService.SelectShootingBlitzRtpConfig(request.Name, request.RtpConfigId);
                    return selectResult 
                        ? ShootingBlitzConfigResult.Ok("RTP config selected successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to select RTP config");

                case ShootingBlitzActionType.QueryTeams:
                    var teams = await shootingBlitzTeamService.GetAllAsync();
                    return ShootingBlitzConfigResult.OkTeams(teams as IList<ShootingBlitzTeam> ?? new List<ShootingBlitzTeam>(teams));

                case ShootingBlitzActionType.CreateTeam:
                    var newTeam = new ShootingBlitzTeam
                    {
                        TeamId = request.TeamId ?? 0,
                        Name = request.TeamName,
                        IsEnabled = request.IsEnabled ?? true,
                        PlayerList = request.PlayerList
                    };
                    var createdTeam = await shootingBlitzTeamService.CreateAsync(newTeam);
                    return createdTeam != null 
                        ? ShootingBlitzConfigResult.OkTeam(createdTeam) 
                        : ShootingBlitzConfigResult.Fail("Failed to create team");

                case ShootingBlitzActionType.UpdateTeam:
                    var teamToUpdate = new ShootingBlitzTeam
                    {
                        TeamId = request.TeamId ?? 0,
                        Name = request.TeamName,
                        IsEnabled = request.IsEnabled ?? true,
                        PlayerList = request.PlayerList
                    };
                    var updateTeamResult = await shootingBlitzTeamService.UpdateAsync(request.TeamDbId, teamToUpdate);
                    return updateTeamResult 
                        ? ShootingBlitzConfigResult.Ok("Team updated successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to update team");

                case ShootingBlitzActionType.DeleteTeam:
                    var deleteTeamResult = await shootingBlitzTeamService.DeleteAsync(request.TeamDbId);
                    return deleteTeamResult 
                        ? ShootingBlitzConfigResult.Ok("Team deleted successfully") 
                        : ShootingBlitzConfigResult.Fail("Failed to delete team");

                default:
                    return ShootingBlitzConfigResult.Fail($"Unknown action type: {request.ActionType}");
            }
        }
    }
}

