using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Api.UM.Requests;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IGameServerApi
    {
        [Post("/api/um")]
        Task UnderMaintenance(ServerApiUMRequest request);

        [Post("/api/reload")]
        Task Reload();

        [Post("/api/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] JWTRequest request);

        [Get("/api/onlineusers")]
        Task<OnlineUsers> GetAllOnlineUsers();

        [Post("/api/proviet-keno/round-result")]
        Task NotifyRoundResult([Body] ProVietKenoRound kenoRound);
    }
}
