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
    [Route("api/shooting_blitz_rtp")]
    public class ShootingBlitzRtpConfigController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ShootingBlitzRtpConfigController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShootingBlitzRtpConfig>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await mediator.Send(new GetAllShootingBlitzRtpConfigRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShootingBlitzRtpConfig), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await mediator.Send(new GetShootingBlitzRtpConfigByIdRequest(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShootingBlitzRtpConfig), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateShootingBlitzRtpConfigRequest request)
        {
            var result = await mediator.Send(request);
            if (result == null)
            {
                return BadRequest("Failed to create RTP config");
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateShootingBlitzRtpConfigRequest request)
        {
            request.Id = id;
            var result = await mediator.Send(request);
            if (!result)
            {
                return BadRequest("Failed to update RTP config");
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await mediator.Send(new DeleteShootingBlitzRtpConfigRequest(id));
            if (!result)
            {
                return BadRequest("Failed to delete RTP config");
            }
            return Ok(result);
        }
    }
}

