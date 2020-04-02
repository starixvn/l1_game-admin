using GamesAdmin.Api.GameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Games
{
    [ApiController, Authorize]
    [Route("api/game-settings")]
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
      
        [HttpPost]
        public Task<bool> Create(GameConfig game)
            => mediator.Send(new CreateRequest(game));

        [HttpPut]
        public Task<bool> Update(GameConfig game)
            => mediator.Send(new UpdateRequest(game));

        [HttpPost("{name}/delete")]
        public Task<bool> Delete(string name)
            => mediator.Send(new DeleteRequest(name));

        [HttpGet("{name}")]
        public Task<GameConfig> GetByName(string name)
        => mediator.Send(new GetByNameRequest(name));
    }
}
