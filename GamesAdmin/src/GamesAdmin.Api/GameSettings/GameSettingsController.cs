using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Games
{
    [ApiController]
    [Authorize]
    [Route("api/game-settings")]
    public class GameSettingsController : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(GameConfig game)
        {
            if (string.IsNullOrWhiteSpace(game.Name))
            {
                return BadRequest();
            }

            var created = await mediator.Send(new CreateRequest(game));

            if (created)
            {
                return CreatedAtAction(nameof(GetByName), new { name = game.Name }, game);
            }

            return BadRequest();
        }

        [HttpPut]
        public Task<bool> Update(GameConfig game)
            => mediator.Send(new UpdateRequest(game));

        [HttpDelete("{name}/delete")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var deleted = await mediator.Send(new DeleteRequest(name));

            if (!deleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(GameConfig), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name)
        {
            var gameConfig = await mediator.Send(new GetByNameRequest(name));

            if (gameConfig != default)
            {
                return Ok(gameConfig);
            }

            return NotFound();
        }
    }
}
