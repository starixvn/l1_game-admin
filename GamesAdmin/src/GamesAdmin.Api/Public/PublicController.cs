using System.Threading.Tasks;
using GamesAdmin.Api.Public.Models;
using GamesAdmin.Api.Public.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.Public
{
    [Route("api/public")]
    public class PublicController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublicController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("game_configs/{gameId}")]
        public Task<GameConfig> GetById(byte gameId)
        => mediator.Send(new GetGameConfigRequest(gameId));
    }
}