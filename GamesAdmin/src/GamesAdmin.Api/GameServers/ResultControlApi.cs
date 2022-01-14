using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models.SlotGame;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface IResultControlApi
    {
        [Post("/api/set-result")]
        Task<SettlementBoard> SetResult([Body] SetResultRequest request);
    }

    public interface IHiLoApi : IResultControlApi
    {
    }
}