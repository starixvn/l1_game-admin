using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.ClientSites.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Sites
{
    [Route("api/client_sites")]
    public class ClientSiteController : BaseAuthorizeController
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
        public Task<bool> Update(ClientSite clientSite)
            => mediator.Send(new UpdateRequest(clientSite));

        [HttpGet]
        [Route("{clientId}/{siteId}")]
        public async Task<ClientSite> Get(string clientId, string siteId)
           => (await mediator.Send(new GetRequest(clientId, siteId)));
    }
}