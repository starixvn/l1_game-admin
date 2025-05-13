

using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameSettings.JackpotSettingsRequests;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameSettings
{
    [Route("api/jackpot-settings")]
    public class JackpotSettingsController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public JackpotSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("update-bot-ratio-settings")]
        public Task<UpdateJackpotSettingsResult> UpdateBotRatioSettings(JackpotSettings settings)
            => mediator.Send(new UpdateBotRatioSettingsRequest { Settings = settings });

        [HttpPost("update-bot-settings")]
        public Task<UpdateJackpotSettingsResult> UpdateBotSettings(JackpotSettings settings)
            => mediator.Send(new UpdateBotSettingsRequest { Settings = settings });

        [HttpPost("update-player-settings")]
        public Task<UpdateJackpotSettingsResult> UpdatePlayerSettings(JackpotSettings settings)
            => mediator.Send(new UpdatePlayerSettingsRequest { Settings = settings });
    }
}