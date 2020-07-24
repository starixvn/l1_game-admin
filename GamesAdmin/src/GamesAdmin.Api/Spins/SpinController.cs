using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Spins.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Spins
{
    [Route("api/spin")]
    public class SpinController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public SpinController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public Task Add([FromBody] CreateRequest request)
        => mediator.Send(request);
    }
}
