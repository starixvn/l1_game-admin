using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameGroupManagement.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameGroupManagement
{
    [Route("api/gamegroup")]
    public class GameGroupManagementController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GameGroupManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut]
        public Task<bool> Update(GameGroup gameGroup)
           => mediator.Send(new UpdateRequest { GameGroup = gameGroup });

        [HttpPost]
        public Task<bool> Create(GameGroup gameGroup)
        => mediator.Send(new CreateRequest { GameGroup = gameGroup });

        [HttpGet]
        [Route("{groupId}")]
        public Task<GameGroup> Get(string groupId)
            => mediator.Send(new GetByGroupIdRequest { GroupId = groupId });

        [HttpGet("all")]
        public Task<IEnumerable<GameGroup>> GetAll()
            => mediator.Send(new GetAllRequest());

        [HttpGet("getbygameid")]
        public Task<GameGroup> GetByGameId(int gameId)
            => mediator.Send(new GetByGameIdRequest { GameId = gameId });
    }
}