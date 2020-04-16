using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Dashboard.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Dashboard
{

    [Route("api/dashboard")]
    public class DashboardController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public DashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("TodayTotalBets")]
        public async Task<IActionResult> TodayTotalBets()
        {
            var totalBets = await mediator.Send(new GetTodayTotalBetRequest());

            return Ok(totalBets);
        }

    }
}
