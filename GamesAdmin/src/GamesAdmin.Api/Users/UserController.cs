using GamesAdmin.Api.Users.Requests;
using GamesAdmin.Api.Users.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Users
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
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
        public async Task<IList<GenerateUserResult>> GenerateBotsRandom(string[] names)
        {
            if (names?.Length < 0 || names?.Length > 20)
            {
                throw new InvalidOperationException("Number of generated users should be in range [1..20]");
            }
        
            return await mediator.Send(new GenerateUserRandomRequest(names, true));
        }

        [HttpPost("generate")]
        public async Task<IList<GenerateUserResult>> GenerateUsersRandom(string[] names)
        {
            if (names?.Length < 0 || names?.Length > 20)
            {
                throw new InvalidOperationException("Number of generated users should be in range [1..20]");
            }

            return await mediator.Send(new GenerateUserRandomRequest(names, false));
        }
    }
}