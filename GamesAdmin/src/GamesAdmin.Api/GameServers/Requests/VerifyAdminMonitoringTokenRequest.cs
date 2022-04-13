using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class VerifyAdminMonitoringTokenRequest : IRequest<bool>
    {
        public VerifyAdminMonitoringTokenRequest(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}