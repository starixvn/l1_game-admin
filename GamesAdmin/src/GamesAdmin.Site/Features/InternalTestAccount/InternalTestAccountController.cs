namespace GamesAdmin.Site.Features.InternalTestAccount
{
    using Audit.Mvc;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class InternalTestAccountController : Controller
    {
        private readonly IMediator mediator;

        public InternalTestAccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
         => View(await mediator.Send(new GetInternalTestAccountRequest()));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string[] addAccounts, string[] deletedAccounts)
        {
            var result = await mediator.Send(new UpdateInternalTestAccountRequest(addAccounts, deletedAccounts));
           
            return Json(result);
        }

    }
}
