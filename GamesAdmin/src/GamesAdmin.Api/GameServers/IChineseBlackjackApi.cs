using GamesAdmin.Api.GameServers.Requests;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IChineseBlackjackApi
    {            
        [Post("/api/set-result")]
        Task<bool> SetResult([Body] SetChineseBlackjackResultRequest request);
    }
}
