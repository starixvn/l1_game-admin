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
        public async Task<IEnumerable<VietlottKenoRound>> GetRoundsAsync(DateTime date)
            => (await mediator.Send(new GetRoundsRequest(date)));
    }
}