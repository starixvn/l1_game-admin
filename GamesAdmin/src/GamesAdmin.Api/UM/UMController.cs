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
        public Task<bool> Schedule(DateTime startTime, DateTime endTime)
        {
            if (startTime == default || endTime == default)
            {
                throw new ArgumentException($"{nameof(startTime)} and {nameof(endTime)} should not be null");
            }

            return mediator.Send(new ScheduleRequest(startTime, endTime));
        }

        [HttpPost("/start")]
        public Task<bool> Start()
        => mediator.Send(new StartRequest());

        [HttpPost("/end")]
        public Task<bool> End()
        => mediator.Send(new EndRequest());
    }
}