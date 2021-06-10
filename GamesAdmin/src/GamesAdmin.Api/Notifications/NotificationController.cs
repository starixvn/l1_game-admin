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
        public async Task<bool> Add([FromBody]AddRequest addRequest)
        {
            if (string.IsNullOrWhiteSpace(addRequest.Message))
            {
                return false;
            }

            return await mediator.Send(addRequest);
        }

        [HttpGet("{id}")]
        public Task<Notification> Get(string id) => mediator.Send(new GetByIdRequest(id));

        [HttpPost]
        [Route("update")]
        public async Task<bool> Update([FromBody]UpdateRequest updateRequest)
        {
            if (updateRequest.Messages.Count == 0)
            {
                return false;
            }

            return await mediator.Send(updateRequest);
        }

        [HttpPost("delete/{id}")]
        public Task<bool> Delete(string id) => mediator.Send(new DeleteRequest(id));
    }
}