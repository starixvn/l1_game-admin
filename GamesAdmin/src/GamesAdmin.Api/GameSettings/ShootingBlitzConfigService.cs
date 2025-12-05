using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.Games;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using Sentry;

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
        private readonly ISentryClient sentryClient;

        public ShootingBlitzConfigService(
            IGameSettingsService gameService,
            IShootingBlitzRtpConfigService shootingBlitzRtpConfigService,
            IShootingBlitzTeamService shootingBlitzTeamService,
            ISentryClient sentryClient)
        {
            this.gameService = gameService;
            this.shootingBlitzRtpConfigService = shootingBlitzRtpConfigService;
            this.shootingBlitzTeamService = shootingBlitzTeamService;
            this.sentryClient = sentryClient;
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
                    try
                    {
                        sentryClient.CaptureMessage($"CreateTeam: TeamId={request.TeamId}, TeamName={request.TeamName}, IsEnabled={request.IsEnabled}");

                        // Validate required fields
                        if (request.TeamId == null || request.TeamId <= 0)
                        {
                            sentryClient.CaptureMessage("CreateTeam validation failed: TeamId is null or <= 0");
                            return ShootingBlitzConfigResult.Fail("Team ID is required and must be greater than 0");
                        }

                        if (string.IsNullOrWhiteSpace(request.TeamName))
                        {
                            sentryClient.CaptureMessage("CreateTeam validation failed: TeamName is null or empty");
                            return ShootingBlitzConfigResult.Fail("Team Name is required");
                        }

                        // Check if TeamId already exists
                        var teamIdExists = await shootingBlitzTeamService.IsTeamIdExistsAsync(request.TeamId.Value);
                        sentryClient.CaptureMessage($"CreateTeam: IsTeamIdExistsAsync({request.TeamId.Value}) = {teamIdExists}");
                        
                        if (teamIdExists)
                        {
                            sentryClient.CaptureMessage($"CreateTeam failed: Team with Team ID {request.TeamId.Value} already exists");
                            return ShootingBlitzConfigResult.Fail($"Team with Team ID {request.TeamId.Value} already exists");
                        }

                        var newTeam = new ShootingBlitzTeam
                        {
                            TeamId = request.TeamId.Value,
                            Name = request.TeamName,
                            IsEnabled = request.IsEnabled ?? true,
                            PlayerList = request.PlayerList
                        };
                        
                        sentryClient.CaptureMessage($"CreateTeam: Calling CreateAsync with TeamId={newTeam.TeamId}, Name={newTeam.Name}");
                        var createdTeam = await shootingBlitzTeamService.CreateAsync(newTeam);
                        
                        if (createdTeam == null)
                        {
                            sentryClient.CaptureMessage("CreateTeam failed: CreateAsync returned null");
                            return ShootingBlitzConfigResult.Fail("Failed to create team. Please check server logs for details.");
                        }

                        sentryClient.CaptureMessage($"CreateTeam success: Created team with Id={createdTeam.Id}, TeamId={createdTeam.TeamId}");
                        return ShootingBlitzConfigResult.OkTeam(createdTeam);
                    }
                    catch (Exception ex)
                    {
                        sentryClient.CaptureException(ex);
                        return ShootingBlitzConfigResult.Fail($"Failed to create team: {ex.Message}");
                    }

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

