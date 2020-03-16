using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api.Games.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Games
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IGameService gamesService;

        public GamesController(IMediator mediator, IGameService gamesService)
        {
            this.mediator = mediator;
            this.gamesService = gamesService;
        }

        [HttpGet]
        public async Task<IList<Game>> Get()
           => (await mediator.Send(new GetAllRequest())).ToList();

        [HttpGet("{id:length(24)}")]
        public Task<Game> Get(string id)
            => mediator.Send(new GetByIdRequest(id));

        [HttpPost]
        public Task<bool> Create(Game game)
            => mediator.Send(new CreateRequest(game));

        [HttpPut]
        public Task<bool> Update(Game game)
            => mediator.Send(new UpdateRequest(game));
    }
}
