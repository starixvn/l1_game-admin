using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Accounts.Requests;
using GamesAdmin.Api.Accounts.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Accounts
{
    [Route("api/accounts")] 
    public class AccountController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
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
    }
}