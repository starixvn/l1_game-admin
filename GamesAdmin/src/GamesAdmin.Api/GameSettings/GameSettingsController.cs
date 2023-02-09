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
        public async Task<bool> Create(GameConfig game)
        {
            if (string.IsNullOrWhiteSpace(game.Info.Name))
            {
                return false;
            }

            return await mediator.Send(new CreateRequest(game));
        }

        [HttpPost("clone_slot_game")]
        public Task<string> ClonelotGame(byte gameId, byte fromGameId, string displayName)
            => mediator.Send(new CloneNewSlotGameRequest(gameId, fromGameId, displayName));

        [HttpGet("{name}")]
        public Task<GameConfig> GetByName(string name)
            => mediator.Send(new GetByNameRequest(name));

        [HttpPut("update_status")]
        public Task<bool> UpdateStatus(string[] names, bool enabled)
           => mediator.Send(new UpdateStatusRequest(names, enabled));

        [HttpPut("update_message")]
        public Task<bool> UpdateMessage(UpdateDisabledMessageRequest request)
           => mediator.Send(request);

        [HttpPost("{name}/update_bot_ratios")]
        public Task<bool> UpdateBetLimitAndOdds(string name, double[] ratios)
           => mediator.Send(new UpdateBotRatiosRequest(name, ratios));

        [HttpPost("update_odds")]
        public Task<bool> UpdateBetLimitAndOdds(UpdateOddsRequest request)
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

        [HttpPost("update_slot_config")]
        public Task<bool> UpdateSlotConfig(UpdateSlotConfigRequest request)
           => mediator.Send(request);

        [HttpPost("update_cascading_slots_config")]
        public Task<bool> UpdateCascadingSlotsConfig(UpdateCascadingSlotsConfigRequest request)
           => mediator.Send(request);

        [HttpPost("update_slot_rng_config")]
        public Task<bool> UpdateCascadingSlotsConfig(UpdateSlotRNGConfigRequest request)
          => mediator.Send(request);

        [HttpPost("{name}/update_slot_odds")]
        public Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds)
           => mediator.Send(new UpdateSlotIconOddsRequest(name, iconOdds));

        [HttpPost("{name}/update_cascading_slot_odds")]
        public Task<bool> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig)
            => mediator.Send(new UpdateOddsByCountsRequest(name, cascadingSlotsConfig));

        [HttpGet("{name}/disabled_message")]
        public Task<Dictionary<string, string>> GetDisableMessage(string name)
            => mediator.Send(new GetDisabledMessageRequest(name));
    }
}