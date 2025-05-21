using GamesAdmin.Site.Features.JackpotSettingsManagement.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.JackpotSettingsManagement.Requests
{
    public class UpdatePageRequest : IRequest<JackpotSettingsViewModel>
    {
        public string ConfigId { get; set; }
    }
}