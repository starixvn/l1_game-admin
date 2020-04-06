using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api.Sites.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Sites
{
    [Route("api/sites")]
    [ApiController]
    public class SiteController : Controller
    {
        private readonly IMediator mediator;

        public SiteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Site>> GetAll()
           => (await mediator.Send(new GetAllRequest()));

        [HttpPost("update")]
        public Task<bool> Update(Site site)
            => mediator.Send(new UpdateRequest(site));
    }
}