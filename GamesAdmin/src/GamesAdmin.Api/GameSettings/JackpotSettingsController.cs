

using System.Collections.Generic;
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

        [HttpGet("getall")]
        public Task<IEnumerable<JackpotSettings>> GetAllJackpotSettings()
            => mediator.Send(new GetAllJackpotSettingsRequest());

        [HttpGet("get/{configId}")]
        public Task<JackpotSettings> GetJackpotSettings(string configId)
            => mediator.Send(new GetJackpotSettingsRequest { ConfigId = configId });

        [HttpGet("search")]
        public Task<IEnumerable<JackpotSettings>> Search([FromQuery] SearchRequest request)
            => mediator.Send(request);
    }
}