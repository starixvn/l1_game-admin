using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class GetAdminMonitoringSettingsRequest : IRequest<AdminMonitoringModel>
    {
        public GetAdminMonitoringSettingsRequest(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}