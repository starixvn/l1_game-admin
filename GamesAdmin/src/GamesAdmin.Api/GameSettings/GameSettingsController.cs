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
    [Route("game-settings")]
    public class GameSettingsController : ControllerBase
    {
        private readonly IMediator mediator;

        public GameSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
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

        [HttpGet("{name}")]
        public Task<GameConfig> GetByName(string name)
        => mediator.Send(new GetByNameRequest(name));
    }
}
