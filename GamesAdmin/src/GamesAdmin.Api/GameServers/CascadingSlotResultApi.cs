using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models.SlotGame;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface ICascadingSlotResultApi
    {
        [Post("/api/set-result")]
        Task<CascadingSettlementResult> SetResult([Body] SetCascadingSlotResultServerRequest request);
    }

    public interface ITreasureIslandResultApi : ICascadingSlotResultApi
    {
        [Post("/api/set-result")]
        new Task<CascadingSettlementResult> SetResult([Body] SetCascadingSlotResultServerRequest request);
    }
}