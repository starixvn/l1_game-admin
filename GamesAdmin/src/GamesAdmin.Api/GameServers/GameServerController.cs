using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using GamesAdmin.Core.Models.SlotGame;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    [Route("api/game_server")]
    public class GameServerController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GameServerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("reload")]
        public Task<bool> Reload()
           => mediator.Send(new ReloadRequest());

        [HttpPost("jwt")]
        public Task<JsonWebToken> GetJWT([FromBody] JWTRequest request)
           => mediator.Send(request);

        [HttpGet("online_users/{game?}")]
        public Task<OnlineUsers> OnlineUsers(string game)
           => mediator.Send(new OnlineUserRequest(game));

        [HttpPost("notify_proviet_results")]
        public Task<bool> NotifyRoundResult([FromBody] IEnumerable<ProVietKenoRound> kenoRounds)
           => mediator.Send(new NotifyRoundsRequest(kenoRounds));

        [HttpPost("slot/set_board")]
        public Task<SettlementBoard> SetSlotResult([FromBody] SetSlotBoardRequest request)
           => mediator.Send(request);

        [HttpPost("holdempoker/set_result")]
        public Task<HoldemPokerResultControl> SetHoldemPokerResult([FromBody] SetHoldemPokerResultRequest request)
           => mediator.Send(request);

        [HttpPost("get_admin_monitoring_settings")]
        public Task<AdminMonitoringModel> GetAdminMonitoringSettings(string username)
         => mediator.Send(new GetAdminMonitoringSettingsRequest(username));

        [HttpPost("hilo/set_result")]
        public Task<IEnumerable<PokerCard>> SetHiLoResult([FromBody] SetHiLoResultRequest request)
        => mediator.Send(request);

        [HttpPost("verify_admin_monitoring_token")]
        public Task<bool> VerifyAdminMonitoringToken(string token)
          => mediator.Send(new VerifyAdminMonitoringTokenRequest(token));
    }
}