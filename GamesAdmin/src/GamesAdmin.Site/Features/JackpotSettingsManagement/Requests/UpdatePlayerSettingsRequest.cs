
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Requests
{
    public class UpdatePlayerSettingsRequest : IRequest<UpdateJackpotSettingsResult>
    {
        public JackpotSettingsViewModel Settings { get; set; }
    }
}
