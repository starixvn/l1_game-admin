using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
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

        [HttpGet("{name}")]
        public Task<GameConfig> GetByName(string name)
            => mediator.Send(new GetByNameRequest(name));

        [HttpPut("update_status")]
        public Task<bool> UpdateStatus(string[] names, bool enabled)
           => mediator.Send(new UpdateStatusRequest(names, enabled));

        [HttpPut("{name}/update_message")]
        public Task<bool> UpdateMessage(string name, string disabledMessage)
           => mediator.Send(new UpdateDisabledMessageRequest(name, disabledMessage));

        [HttpPut("update_message_all")]
        public Task<bool> UpdateMessage(string disabledMessage)
           => mediator.Send(new UpdateDisabledMessageRequest(string.Empty, disabledMessage, true));

        [HttpPost("{name}/update_bot_ratios")]
        public Task<bool> UpdateBetLimitAndOdds(string name, double[] ratios)
           => mediator.Send(new UpdateBotRatiosRequest(name, ratios));

        [HttpPost("update_bet_limit_odds")]
        public Task<bool> UpdateBetLimitAndOdds(UpdateBetLimitAndOddsRequest request)
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

        [HttpPost("{name}/update_slot_odds")]
        public Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds)
           => mediator.Send(new UpdateSlotIconOddsRequest(name, iconOdds));

        [HttpPost("{name}/update_site_exclusions")]
        public Task<bool> UpdateSiteExclusions(string name, string[] exclusions)
          => mediator.Send(new UpdateSiteExclusionRequest(name, exclusions));
    }
}