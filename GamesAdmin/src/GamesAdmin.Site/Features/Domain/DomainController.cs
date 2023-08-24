namespace GamesAdmin.Site.Features.Domain
{
    using Audit.Mvc;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class DomainController : Controller
    {
        private readonly IMediator mediator;

        public DomainController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
         => View(await mediator.Send(new GetDomainRequest()));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string id, string value)
        {
            var result = await mediator.Send(new UpdateDomainRequest(id, value));

            return Ok(result);
        }
    }
}
