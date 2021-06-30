using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Api.GlobalConfigs;
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
        , IRequestHandler<UpdateBetLimitAndOddsRequest, bool>
        , IRequestHandler<UpdateBotRatiosRequest, bool>
        , IRequestHandler<UpdateGeneralConfigRequest, bool>
        , IRequestHandler<UpdateJackpotRequest, bool>
        , IRequestHandler<UpdateSlotConfigRequest, bool>
        , IRequestHandler<UpdateSlotIconOddsRequest, bool>
        , IRequestHandler<GetDisabledMessageRequest, Dictionary<string, string>>
    {
        private readonly IGameSettingsService gameService;
        private readonly IGlobalConfigService globalConfigService;

        public GameSettingsHandler(IGameSettingsService gamesService, IGlobalConfigService globalConfigService)
        {
            this.gameService = gamesService;
            this.globalConfigService = globalConfigService;
        }

        public Task<IEnumerable<GameConfig>> Handle(GetAllRequest request, CancellationToken cancellationToken)
         => gameService.Get();

        public async Task<bool> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var createResult = await gameService.Create(request.Game);
            await globalConfigService.UpdateSiteExclusions(request.Game.Info.Name, new string[] { });

            return createResult;
        }

        public Task<GameConfig> Handle(GetByNameRequest request, CancellationToken cancellationToken)
        => gameService.GetByName(request.Name);

        public Task<bool> Handle(UpdateStatusRequest request, CancellationToken cancellationToken)
        => gameService.UpdateStatus(request.Names, request.Enabled);

        public Task<bool> Handle(UpdateDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.UpdateDisbledMessage(request.Name, request.Messages, request.IsApplyAll);

        public Task<bool> Handle(UpdateBetLimitAndOddsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBetLimitAndOdds(request.Name, request.BetLimit, request.Odds);

        public Task<bool> Handle(UpdateBotRatiosRequest request, CancellationToken cancellationToken)
        => gameService.UpdateBotRatios(request.Name, request.Ratios);

        public Task<bool> Handle(UpdateGeneralConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateGeneralConfig(request.GameInfo, request.BotSetting, request.EnableSetting);

        public Task<bool> Handle(UpdateJackpotRequest request, CancellationToken cancellationToken)
        => gameService.UpdateJackpot(request.Name, request.Jackpot, request.StakeLevels);

        public Task<bool> Handle(UpdateSlotConfigRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotConfig(request.Name, request.BotEnabled, request.DisplayName, request.SlotsConfig);

        public Task<bool> Handle(UpdateSlotIconOddsRequest request, CancellationToken cancellationToken)
        => gameService.UpdateSlotIconOdds(request.Name, request.IconOdds);

        public Task<Dictionary<string, string>> Handle(GetDisabledMessageRequest request, CancellationToken cancellationToken)
        => gameService.GetDisaledMessage(request.Name);
    }
}