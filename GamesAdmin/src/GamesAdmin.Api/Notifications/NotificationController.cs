using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Notifications
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IList<Notification>> Get() => mediator.Send(new GetRequest());

        [HttpPost]
        [Route("add")]
        public async Task<bool> Add([FromBody]CreateRequest createRequest)
        {
            if (string.IsNullOrWhiteSpace(createRequest.Message))
            {
                return false;
            }

            return await mediator.Send(createRequest);
        }
    }
}