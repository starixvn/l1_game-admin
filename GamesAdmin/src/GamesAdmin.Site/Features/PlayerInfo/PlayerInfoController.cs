namespace GamesAdmin.Site.Features.PlayerInfo
{
    using Audit.Mvc;
    using ViewModels;
    using Requests;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class PlayerInfoController : Controller
    {
        private readonly IMediator mediator;

        public PlayerInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index()
         => View(new IndexViewModel());

        [HttpPost, Audit]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            var result = await mediator.Send(new GetPlayerInfoRequest(model.PlayerId, model.Name));

            return View(result);
        }
    }
}
