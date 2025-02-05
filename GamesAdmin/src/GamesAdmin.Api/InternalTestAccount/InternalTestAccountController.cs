namespace GamesAdmin.Api.InternalTestAccount
{
    using GamesAdmin.Api._Shared;
    using GamesAdmin.Api.InternalTestAccount.Requests;
    using GamesAdmin.Core.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/internalTestAccount")]
    public class InternalTestAccountController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public InternalTestAccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var accounts = (await mediator.Send(new GetInternalTestAccountRequest()));

            return Ok(accounts);
        }

        [HttpPost("update")]
        public Task<UpdateInternalUUSTestResult> Update(UpdateInternalTestAccountRequest model)
         => mediator.Send(model);
    }
}
