using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.UM.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.UM
{
    [Route("api/um")]   
    public class UMController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public UMController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost()]
        public Task<bool> Start([FromBody] UMRequest request)
        => mediator.Send(request);
    }
}