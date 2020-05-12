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
            if (string.IsNullOrWhiteSpace(game.Name))
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

        [HttpPut("{name}/update_status")]
        public Task<bool> UpdateStatus(string name, bool enabled)
           => mediator.Send(new UpdateStatusRequest(name, enabled));

        [HttpPut("{name}/update_message")]
        public Task<bool> UpdateMessage(string name, string disabledMessage)
           => mediator.Send(new UpdateDisabledMessageRequest(name, disabledMessage));
    }
}
