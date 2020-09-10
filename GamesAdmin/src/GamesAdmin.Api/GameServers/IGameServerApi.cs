using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    public interface IGameServerApi
    {
        [Post("/api/um")]
        Task UnderMaintenance(UnderMaintenanceRequest request);
    }
}
