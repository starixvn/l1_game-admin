using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IUserServiceApi
    {
        [Post("/jwt")]
        Task<JsonWebToken> GetToken([Body(BodySerializationMethod.UrlEncoded)] JWTRequest request);

        [Get("/admin/online-users")]
        Task<OnlineUsers> GetAllOnlineUsers();
    }
}
