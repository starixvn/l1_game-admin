using GamesAdmin.Api.UM.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    [Route("api/um")]
    [ApiController, Authorize]
    public class UMController : ControllerBase
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