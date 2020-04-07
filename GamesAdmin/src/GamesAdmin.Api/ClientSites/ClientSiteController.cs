using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.ClientSites.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Sites
{
    [Route("api/clientsites")]
    [ApiController]
    public class ClientSiteController : Controller
    {
        private readonly IMediator mediator;

        public ClientSiteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientSite>> GetAll()
           => (await mediator.Send(new GetAllRequest()));

        [HttpPost("update")]
        public Task<bool> Update(ClientSite site)
            => mediator.Send(new UpdateRequest(site));
    }
}