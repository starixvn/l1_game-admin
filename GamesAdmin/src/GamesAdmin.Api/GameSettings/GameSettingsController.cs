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

        [HttpPut]
        public Task<bool> Update(GameConfig game)
            => mediator.Send(new UpdateRequest(game));

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

        [HttpPost("{name}/update_bet_limit_odds")]
        public Task<bool> UpdateBetLimitAndOdds(string name, BetLimit betLimit, Dictionary<string, double> odds)
           => mediator.Send(new UpdateBetLimitAndOddsRequest(name, betLimit, odds));

        [HttpPost("{name}/update_bot_ratios")]
        public Task<bool> UpdateBetLimitAndOdds(string name, double[] ratios)
           => mediator.Send(new UpdateBotRatiosRequest(name, ratios));

        [HttpPost("{name}/update_general_settings")]
        public Task<bool> UpdateGeneralSettings(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
           => mediator.Send(new UpdateGeneralSettingRequest(name, gameInfo, botSetting, enableSetting));

        [HttpPost("{name}/update_jackpot")]
        public Task<bool> UpdateGeneralSettings(string name, Jackpot jackpot, List<StakeLevel> stakeLevels)
           => mediator.Send(new UpdateJackpotRequest(name, jackpot, stakeLevels));

        [HttpPost("{name}/update_slot_settings")]
        public Task<bool> UpdateSlotSettings(string name, bool botEnabled, SlotsConfig slotsConfig)
           => mediator.Send(new UpdateSlotConfigRequest(name, botEnabled, slotsConfig));

        [HttpPost("{name}/update_slot_odds")]
        public Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, float>> iconOdds)
           => mediator.Send(new UpdateSlotIconOddsRequest(name, iconOdds));
    }
}