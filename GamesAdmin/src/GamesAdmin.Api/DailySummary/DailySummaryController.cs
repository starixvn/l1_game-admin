using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.DailySummary.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.DailySummarize
{
    [Route("api/daily_summary")]
    public class DailySummaryController : BaseAuthorizeController
    {
        private IMediator mediator;

        public DailySummaryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DailySummaryRequest request)
            => Ok(new { summary = await mediator.Send(request)});
    }
}