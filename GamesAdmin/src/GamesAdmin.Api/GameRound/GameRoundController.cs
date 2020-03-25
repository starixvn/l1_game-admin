using GamesAdmin.Api.GameRound.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameRound
{
    [ApiController]
    [Route("game-rounds")]
    public class GameRoundController : ControllerBase
    {
        private readonly IMediator mediator;

        public GameRoundController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IList<Round>> Get()
           => (await mediator.Send(new GetAllRequest())).ToList();

        [HttpGet()]
        [Route("latest")]
        public Task<Round> GetLatest()
            => mediator.Send(new GetLatestRoundRequest());

        [HttpGet()]
        [Route("bet-amount")]
        public Task<IEnumerable<BetInfo>> GetBetAmount()
            => mediator.Send(new GetBetAmountRequest());
    }
}