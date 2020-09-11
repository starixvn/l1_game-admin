using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IGameServerApi
    {
        [Post("/api/um")]
        Task UnderMaintenance(UnderMaintenanceRequest request);

        [Post("/api/reload")]
        Task Reload();

        [Post("/api/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] JWTRequest request);

        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetAllOnlineUsers();
    }
}
