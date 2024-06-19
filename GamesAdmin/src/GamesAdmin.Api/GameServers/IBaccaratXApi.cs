using GamesAdmin.Api.GameServers.Requests;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IBaccaratXApi
    {
        [Post("/api/{name}/set-result")]
        Task<bool> SetResult([Body] SetBaccaratXRersultRequest request, string name = "");
    }
}
