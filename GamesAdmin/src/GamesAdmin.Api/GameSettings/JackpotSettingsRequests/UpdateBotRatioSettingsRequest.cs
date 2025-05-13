
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.JackpotSettingsRequests
{
    public class UpdateBotRatioSettingsRequest : IRequest<UpdateJackpotSettingsResult>
    {
        public JackpotSettings Settings { get; set; }
    }
}
