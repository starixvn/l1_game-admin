using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameUser.Requests;
using GamesAdmin.Api.GameUser.Results;
using GamesAdmin.Database.Entities.Spins;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameUser
{
    [Route("api/game_users")]
    public class GameUserController : BaseAuthorizeController
    {
        private const int maxGenerateUser = 100;

        private readonly IMediator mediator;

        public GameUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("bot/count")]
        public async Task<int> Get()
           => (await mediator.Send(new GetBotCountRequest()));

        [HttpPost("bots/generate")]
        public async Task<IList<GenerateAccountResult>> GenerateBotsRandom(string[] names)
        {
            if (names?.Length < 0 || names?.Length > maxGenerateUser)
            {
                throw new InvalidOperationException("Number of generated users should be in range [1..100]");
            }

            return await mediator.Send(new GenerateAccountRandomRequest(names));
        }

        [HttpPost("bot/revise_balance")]
        public async Task<int> ReviseBalance()
           => (await mediator.Send(new ReviseBotBalanceRequest()));

        [HttpPost("bot/disable_randomly")]
        public async Task<int> DisableRandomly()
           => (await mediator.Send(new DisableBotRandomlyRequest()));

        [HttpPost("spins/update")]
        public Task<UpdateMemberSpinResult> UpdateMemberSpins([FromBody] UpdateSpinRequest request)
           => mediator.Send(request);

        [HttpGet("spins/{memberId}/get")]
        public Task<SpinEntity> GetMemberSpins(int memberId)
           => mediator.Send(new GetSpinRequest(memberId));

        [HttpGet("spins/{memberId}/get_logs")]
        public Task<IList<SpinLogEntity>> GetMemberSpinLogs(int memberId)
           => mediator.Send(new GetSpinLogRequest(memberId));

        [HttpPost("bot/deactivate")]
        public async Task<int> Deactivate()
           => (await mediator.Send(new DeactivateBotRequest()));
    }
}