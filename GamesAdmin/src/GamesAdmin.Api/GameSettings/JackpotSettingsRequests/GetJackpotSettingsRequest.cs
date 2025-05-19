using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.JackpotSettingsRequests
{
    public class GetJackpotSettingsRequest : IRequest<JackpotSettings>
    {
        public string ConfigId { get; set; }
    }
}
