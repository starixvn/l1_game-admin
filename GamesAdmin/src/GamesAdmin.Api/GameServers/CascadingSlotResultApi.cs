using System.Collections.Generic;
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
        Task<IEnumerable<SettlementCascadingSlotResult>> SetResult([Body] SetCascadingSlotResultRequest request);
    }

    public interface ITreasureIslandResultApi : ICascadingSlotResultApi
    {
        [Post("/api/set-result")]
        new Task<IEnumerable<SettlementCascadingSlotResult>> SetResult([Body] SetCascadingSlotResultRequest request);
    }
}