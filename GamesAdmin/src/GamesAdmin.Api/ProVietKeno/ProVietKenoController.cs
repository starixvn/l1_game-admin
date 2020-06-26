using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.ProVietKeno.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.ProVietKeno
{
    [Route("api/proviet_keno")]
    public class ProVietKenoController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ProVietKenoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("rounds/{date}")]
        public async Task<IEnumerable<ProVietKenoRound>> GetRounds(DateTimeOffset date)
            => (await mediator.Send(new GetRoundsRequest(date)));

        [HttpPost]
        [Route("rounds/create")]
        public async Task<IActionResult> Create([FromBody] CreateProVietKenoRound kenoRound)
        {
            return Ok(await mediator.Send(new CreateRoundRequest(kenoRound)));
        }


        [HttpGet]
        [Route("rounds/id/{roundId}")]
        public async Task<ProVietKenoRound> Get(long roundId)
            => (await mediator.Send(new GetByRoundIdRequest(roundId)));

        [HttpGet]
        [Route("rounds/unsettled")]
        public async Task<IEnumerable<ProVietKenoRound>> GetUnsettledRounds()
          => (await mediator.Send(new GetUnsettledRoundsRequest()));

    }
}