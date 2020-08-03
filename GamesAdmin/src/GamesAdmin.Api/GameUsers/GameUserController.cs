using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameUser
{
    [Route("api/game_users")]
    public class GameUserController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GameUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("bot/count")]
        public async Task<int> Get()
           => (await mediator.Send(new GetBotCountRequest()));

        [HttpPost("bot/revise")]
        public async Task<int> Revise()
           => (await mediator.Send(new ReviseBotNameRequest()));

        [HttpPost("bots/generate")]
        public async Task<IList<GenerateAccountResult>> GenerateBotsRandom(string[] names)
        {
            if (names?.Length < 0 || names?.Length > 100)
            {
                throw new InvalidOperationException("Number of generated users should be in range [1..100]");
            }

            return await mediator.Send(new GenerateAccountRandomRequest(names, true));
        }

        [HttpPost("generate")]
        public async Task<IList<GenerateAccountResult>> GenerateUsersRandom(string[] names)
        {
            if (names?.Length < 0 || names?.Length > 100)
            {
                throw new InvalidOperationException("Number of generated users should be in range [1..100]");
            }

            return await mediator.Send(new GenerateAccountRandomRequest(names, false));
        }

        [HttpPost("bot/revise_balance")]
        public async Task<int> ReviseBalance()
           => (await mediator.Send(new ReviseBotBalanceRequest()));

        [HttpPost("bot/disable_randomly")]
        public async Task<int> DisableRandomly()
           => (await mediator.Send(new DisableBotRandomlyRequest()));

        [HttpPost("add_spins")]
        public async Task<bool> AddSpins([FromBody] AddSpinRequest request)
           => (await mediator.Send(request));
    }
}