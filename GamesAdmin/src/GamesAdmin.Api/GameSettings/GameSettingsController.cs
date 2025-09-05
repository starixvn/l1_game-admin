using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Games
{
    [Route("api/game_settings")]
    public class GameSettingsController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GameSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GameConfig>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var gameConfigs = (await mediator.Send(new GetAllRequest())).ToList();

            return Ok(gameConfigs);
        }

        [HttpPost]
        public async Task<GameCreationResult> Create(GameConfig game)
        {
            if (string.IsNullOrWhiteSpace(game.Info.Name))
            {
                return GameCreationResult.CreateError(
                    GameCreationErrorCodes.InvalidGameData,
                    "Game name is required.");
            }

            return await mediator.Send(new CreateRequest(game));
        }

        [HttpPost("clone_slot_game")]
        public Task<string> ClonelotGame(int gameId, int fromGameId, string displayName, byte gameType)
            => mediator.Send(new CloneNewSlotGameRequest(gameId, fromGameId, displayName, gameType));

        [HttpPost("update_symbol_weight")]
        public Task<bool> UpdateSymbolWeight(UpdateSymbolWeightRequest request)
            => mediator.Send(request);

        [HttpGet("{name}")]
        public Task<GameConfig> GetByName(string name)
            => mediator.Send(new GetByNameRequest(name));

        [HttpPut("update_status")]
        public Task<bool> UpdateStatus(string[] names, bool enabled)
           => mediator.Send(new UpdateStatusRequest(names, enabled));

        [HttpPut("update_message")]
        public Task<bool> UpdateMessage(UpdateDisabledMessageRequest request)
           => mediator.Send(request);

        [HttpPost("update_bot_ratios")]
        public Task<bool> UpdateBotRatios(UpdateBotRatiosRequest request)
           => mediator.Send(request);

        [HttpPost("update_odds")]
        public Task<bool> UpdateBetLimitAndOdds(UpdateOddsRequest request)
           => mediator.Send(request);

        [HttpPost("update_margin")]
        public Task<bool> UpdateMargins(UpdateMarginSettingsRequest request)
            => mediator.Send(request);

        [HttpPost("update_bet_limit")]
        public Task<bool> UpdateBetLimit(UpdateBetLimitRequest request)
           => mediator.Send(request);

        [HttpPost("update_general_config")]
        public Task<bool> UpdateGeneralConfig(UpdateGeneralConfigRequest request)
           => mediator.Send(request);

        [HttpPost("update_jackpot")]
        public Task<bool> UpdateJackpot(UpdateJackpotRequest request)
           => mediator.Send(request);

        [HttpPost("update_slot_bot_config")]
        public Task<bool> UpdateSlotBotConfig(UpdateBotSlotSettingsRequest request)
            => mediator.Send(request);

        [HttpPost("update_slot_config")]
        public Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request)
           => mediator.Send(request);

        [HttpPost("update_cascading_slots_config")]
        public Task<bool> UpdateCascadingSlotsConfig(UpdateCascadingSlotsConfigRequest request)
           => mediator.Send(request);

        [HttpPost("update_slot_rng_config")]
        public Task<bool> UpdateCascadingSlotsConfig(UpdateSlotRNGConfigRequest request)
          => mediator.Send(request);

        [HttpPost("update_crash_config")]
        public Task<bool> UpdateCrashConfig(UpdateCrashConfigRequest request)
          => mediator.Send(request);

        [HttpPost("update_slot_odds")]
        public Task<bool> UpdateSlotIconOdds(UpdateSlotIconOddsRequest request)
           => mediator.Send(request);

        [HttpPost("update_cascading_slot_odds")]
        public Task<bool> UpdateCascadingSlotOdds(UpdateOddsByCountsRequest request)
            => mediator.Send(request);

        [HttpGet("{name}/disabled_message")]
        public Task<Dictionary<string, string>> GetDisableMessage(string name)
            => mediator.Send(new GetDisabledMessageRequest(name));
    }
}