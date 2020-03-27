using GamesAdmin.Api.UM.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GamesAdmin.Api.UM
{
    [Route("api/[controller]")]
    [ApiController]
    public class UMController : ControllerBase
    {
        private readonly IMediator mediator;

        public UMController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/schedule")]
        public Task<bool> Schedule(DateTimeOffset start, DateTimeOffset end)
          => mediator.Send(new ScheduleRequest(start, end));
    }
}