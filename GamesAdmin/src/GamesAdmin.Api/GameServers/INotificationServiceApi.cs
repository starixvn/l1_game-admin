using GamesAdmin.Api.UM.Requests;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    public interface INotificationServiceApi
    {
        [Post("/api/um")]
        Task UnderMaintenance(ServerApiUMRequest request);

        [Post("/api/reload")]
        Task Reload();

        [Get("/api/connectedClients")]
        Task<int> ConnectedClients();
    }
}