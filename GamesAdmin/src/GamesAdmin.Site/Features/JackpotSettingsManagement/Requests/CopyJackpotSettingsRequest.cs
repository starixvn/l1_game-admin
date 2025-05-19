using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Requests
{
    public class CopyJackpotSettingsRequest : IRequest<JackpotSettingsViewModel>
    {
        public string ConfigId { get; set; }
    }
}