using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Models.SlotGame;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface ISlotBoardApi
    {
        [Post("/api/set-board")]
        Task<SettlementBoard> SetBoard([Body] SetSlotBoardServerRequest request);
    }
}