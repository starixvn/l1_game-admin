using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameSettings
{
    [Route("api/shooting_blitz_team")]
    public class ShootingBlitzTeamController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ShootingBlitzTeamController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShootingBlitzTeam>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllShootingBlitzTeamRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShootingBlitzTeam), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await mediator.Send(new GetShootingBlitzTeamByIdRequest(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShootingBlitzTeam), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateShootingBlitzTeamRequest request)
        {
            var result = await mediator.Send(request);
            if (result == null)
            {
                return BadRequest("Failed to create team. TeamId may already exist.");
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateShootingBlitzTeamRequest request)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            if (!result)
            {
                return BadRequest("Failed to update team. TeamId may already exist for another team.");
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await mediator.Send(new DeleteShootingBlitzTeamRequest(id));
            if (!result)
            {
                return BadRequest("Failed to delete team");
            }
            return Ok(result);
        }
    }
}

