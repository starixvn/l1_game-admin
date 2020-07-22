using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameRoundResult.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameRoundResult
{
    [Authorize]
    public class GameRoundResultController : Controller
    {
        private readonly IMediator mediator;

        public GameRoundResultController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var url = await mediator.Send(new GetAccessUrlRequest());

            return Redirect(url);
        }
    }
}