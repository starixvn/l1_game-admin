using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.Games
{
    public class GameSettingsHandler
        : IRequestHandler<GetAllRequest, IEnumerable<GameConfig>>
        , IRequestHandler<CreateRequest, GameCreationResult>
        , IRequestHandler<GetByNameRequest, GameConfig>
        , IRequestHandler<UpdateStatusRequest, bool>
        , IRequestHandler<UpdateDisabledMessageRequest, bool>
        , IRequestHandler<UpdateBetLimitRequest, bool>
        , IRequestHandler<UpdateOddsRequest, bool>
        , IRequestHandler<UpdateMarginSettingsRequest, bool>
        , IRequestHandler<UpdateBotRatiosRequest, bool>
        , IRequestHandler<UpdateGeneralConfigRequest, bool>
        , IRequestHandler<UpdateJackpotRequest, bool>
        , IRequestHandler<UpdateSlotConfigRequest, bool>
        , IRequestHandler<UpdateCascadingSlotsConfigRequest, bool>
        , IRequestHandler<UpdateSlotIconOddsRequest, bool>
        , IRequestHandler<UpdateOddsByCountsRequest, bool>
        , IRequestHandler<GetDisabledMessageRequest, Dictionary<string, string>>
        , IRequestHandler<UpdateSlotRNGConfigRequest, bool>
        , IRequestHandler<CloneNewSlotGameRequest, string>
        , IRequestHandler<UpdateSymbolWeightRequest, bool>
        , IRequestHandler<UpdateCrashConfigRequest, bool>
        , IRequestHandler<UpdateBotSlotSettingsRequest, bool>
        , IRequestHandler<ImportRequest, bool>
        , IRequestHandler<UpdatePlinkoConfigRequest, bool>
        , IRequestHandler<UpdateShootingBlitzConfigRequest, ShootingBlitzConfigResult>
    {
        private readonly IGameSettingsService gameService;
        private readonly ICloneSettingService slotSettingService;
        private readonly IShootingBlitzRtpConfigService shootingBlitzRtpConfigService;
        private readonly IShootingBlitzTeamService shootingBlitzTeamService;

        public GameSettingsHandler(
            IGameSettingsService gameService, 
            ICloneSettingService slotSettingService,
            IShootingBlitzRtpConfigService shootingBlitzRtpConfigService,
            IShootingBlitzTeamService shootingBlitzTeamService)
        {
            this.gameService = gameService;
            this.slotSettingService = slotSettingService;
            this.shootingBlitzRtpConfigService = shootingBlitzRtpConfigService;
            this.shootingBlitzTeamService = shootingBlitzTeamService;
        }

        public Task<IEnumerable<GameConfig>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameService.Get();

        public async Task<GameCreationResult> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var createResult = await gameService.Create(request.Game);

            return createResult;
        }

        public Task<GameConfig> Handle(GetByNameRequest request, CancellationToken cancellationToken)
        => gameService.GetByName(request.Name);

        public Task<bool> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
        => gameService.UpdateStatus(request.Names, request.Enabled);

        public Task<bool> Handle(UpdateDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.UpdateDisabledMessage(request.Name, request.Messages, request.IsApplyAll);

        public Task<bool> Handle(UpdateBetLimitRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBetLimit(request.GameId, request.BetLimit, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateOddsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateOdds(request.Name, request.Odds, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateMarginSettingsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateMarginSettings(request.GameName, request.MarginSettings);

        public Task<bool> Handle(UpdateBotRatiosRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBotRatios(request.Name, request.BotRatios, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateGeneralConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateGeneralConfig(request.GameInfo, request.BotSetting, request.EnableSetting, request.OddsMultipliers, request.OtherGameIdsInGroup,request.TournamentConfig, request.VideoStreamConfig);

        public Task<bool> Handle(UpdateJackpotRequest request, CancellationToken cancellationToken)
        => gameService.UpdateJackpot(request.Name, request.Jackpot, request.StakeLevels, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateBotSlotSettingsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotBotSettings(request.Name, request.Jackpot, request.BetLevels, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateSlotConfigRequest request, CancellationToken cancellationToken)
            => gameService.UpdateSlotConfig(request.Name, request.BotEnabled, request.EnablePromotionRTP, request.SlotsConfig,request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateCascadingSlotsConfigRequest request, CancellationToken cancellationToken)
            => gameService.UpdateCascadingSlotsConfig(request.Name, request.BotEnabled, request.EnablePromotionRTP, request.CascadingSlotsConfig, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateSlotIconOddsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotIconOdds(request.Name, request.IconOdds, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateOddsByCountsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateCascadingSlotOdds(request.Name, request.CascadingSlotsConfig, request.OtherGameIdsInGroup);

        public Task<Dictionary<string, string>> Handle(GetDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.GetDisabledMessage(request.Name);

        public Task<bool> Handle(UpdateSlotRNGConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotRNGConfig(request.GameInfo, request.BotSetting, request.EnableSetting, request.SlotRNGConfig);

        public Task<string> Handle(CloneNewSlotGameRequest request, CancellationToken cancellationToken)
        => slotSettingService.CloneNewGame(request.GameId, request.FromGameId, request.DisplayName, request.GameType);

        public Task<bool> Handle(UpdateSymbolWeightRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSymbolWeight(request.Name, request.SymbolWeight, request.OtherGameIdsInGroup);

        public Task<bool> Handle(UpdateCrashConfigRequest request, CancellationToken cancellationToken)
       => gameService.UpdateCrashConfig(request.GameInfo, request.BotSetting, request.EnableSetting, request.CrashConfig);

        public Task<bool> Handle(ImportRequest request, CancellationToken cancellationToken)
            => gameService.ImportGame(request.Game);

        public Task<bool> Handle(UpdatePlinkoConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdatePlinkoConfig(request.Name, request.PlinkoConfig);

        public async Task<ShootingBlitzConfigResult> Handle(UpdateShootingBlitzConfigRequest request, CancellationToken cancellationToken)
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