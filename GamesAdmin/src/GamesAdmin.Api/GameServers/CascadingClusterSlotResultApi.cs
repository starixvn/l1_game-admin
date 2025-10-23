using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface ICascadingClusterSlotResultApi
    {
        [Post("/api/set-result")]
        Task<object> SetResult([FromBody] SetCascadingClusterSlotRequest request);
    }
}