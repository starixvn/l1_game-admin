using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games
{
    public class GameSettingsHandler
        : IRequestHandler<GetAllRequest, IEnumerable<GameConfig>>
        , IRequestHandler<CreateRequest, bool>
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
    {
        private readonly IGameSettingsService gameService;
        private readonly ICloneSettingService slotSettingService;

        public GameSettingsHandler(IGameSettingsService gameService, ICloneSettingService slotSettingService)
        {
            this.gameService = gameService;
            this.slotSettingService = slotSettingService;
        }

        public Task<IEnumerable<GameConfig>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameService.Get();

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
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
        => gameService.UpdateJackpot(request.Name, request.Jackpot, request.StakeLevels);

        public Task<bool> Handle(UpdateBotSlotSettingsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotBotSettings(request.Name, request.Jackpot, request.BetLevels);

        public Task<bool> Handle(UpdateSlotConfigRequest request, CancellationToken cancellationToken)
            => gameService.UpdateSlotConfig(request.Name, request.BotEnabled, request.SlotsConfig);

        public Task<bool> Handle(UpdateCascadingSlotsConfigRequest request, CancellationToken cancellationToken)
            => gameService.UpdateCascadingSlotsConfig(request.Name, request.BotEnabled, request.CascadingSlotsConfig);

        public Task<bool> Handle(UpdateSlotIconOddsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotIconOdds(request.Name, request.IconOdds);

        public Task<bool> Handle(UpdateOddsByCountsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateCascadingSlotOdds(request.Name, request.CascadingSlotsConfig);

        public Task<Dictionary<string, string>> Handle(GetDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.GetDisabledMessage(request.Name);

        public Task<bool> Handle(UpdateSlotRNGConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotRNGConfig(request.GameInfo, request.BotSetting, request.EnableSetting, request.SlotRNGConfig);

        public Task<string> Handle(CloneNewSlotGameRequest request, CancellationToken cancellationToken)
        => slotSettingService.CloneNewGame(request.GameId, request.FromGameId, request.DisplayName);

        public Task<bool> Handle(UpdateSymbolWeightRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSymbolWeight(request.Name, request.SymbolWeight);

        public Task<bool> Handle(UpdateCrashConfigRequest request, CancellationToken cancellationToken)
       => gameService.UpdateCrashConfig(request.GameInfo, request.BotSetting, request.EnableSetting, request.CrashConfig);

        public Task<bool> Handle(ImportRequest request, CancellationToken cancellationToken)
            => gameService.ImportGame(request.Game);
    }
}