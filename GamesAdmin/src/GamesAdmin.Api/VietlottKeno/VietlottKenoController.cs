using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.VietlottKeno.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.VietlottKeno
{
    [Route("api/vietlott_keno")]
    public class VietlottKenoController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public VietlottKenoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("rounds/{date}")]
        public async Task<IEnumerable<VietlottKenoRound>> GetRounds(DateTime date)
            => (await mediator.Send(new GetRoundsRequest(date)));

        [HttpPost]
        [Route("rounds/create")]
        public async Task<IActionResult> Create([FromBody] CreateVietlottKenoRound kenoRound)
        {
            await mediator.Send(new CreateRoundRequest(kenoRound));
            return Ok();
        }
    }
}