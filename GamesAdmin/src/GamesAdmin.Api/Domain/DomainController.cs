namespace GamesAdmin.Api.Domain
{
    using GamesAdmin.Api._Shared;
    using GamesAdmin.Api.Domain.Requests;
    using GamesAdmin.Core.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/domain")]
    public class DomainController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public DomainController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var domains = (await mediator.Send(new GetDomainRequest()));

            return Ok(domains);
        }

        [HttpPost("update")]
        public Task<bool> Update(DomainModel model)
         => mediator.Send(new UpdateDomainRequest(model));
    }
}
