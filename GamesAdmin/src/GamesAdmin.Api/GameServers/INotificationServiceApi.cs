using System.Threading.Tasks;
using GamesAdmin.Api.UM.Requests;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
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
