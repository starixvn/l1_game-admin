using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetMonitorRequest : IRequest<MonitorViewModel>
    {
        public GetMonitorRequest(string jwtToken)
        {
            JwtToken = jwtToken;
        }

        public string JwtToken { get; set; }
    }
}
