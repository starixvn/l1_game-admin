using System;
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
    [Route("games")]
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
        public async Task<IList<GameConfig>> Get()
           => (await mediator.Send(new GetAllRequest())).ToList();

        [HttpGet("{id:length(36)}")]
        public Task<GameConfig> Get(Guid id)
            => mediator.Send(new GetByIdRequest(id));

        [HttpPost]
        public Task<bool> Create(GameConfig game)
            => mediator.Send(new CreateRequest(game));

        [HttpPut]
        public Task<bool> Update(GameConfig game)
            => mediator.Send(new UpdateRequest(game));

        [HttpPost("{id:length(36)}/delete")]
        public Task<bool> Delete(Guid id)
            => mediator.Send(new DeleteRequest(id));
    }
}
