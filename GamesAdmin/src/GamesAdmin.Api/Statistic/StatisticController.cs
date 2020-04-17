using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Statistic.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Statistic
{
    [Route("api/statistic")]
    public class StatisticController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public StatisticController(IMediator mediator)
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