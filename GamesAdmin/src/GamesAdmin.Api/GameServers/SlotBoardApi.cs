using System.Threading.Tasks;
using GamesAdmin.Api.GameServers.Requests;
using Refit;

namespace GamesAdmin.Api.GameServers
{
    [Headers("Authorization: Bearer")]
    public interface ISlotBoardApi
    {
        [Post("/api/set-board")]
        Task SetBoard([Body] SetSlotBoardRequest request);
    }

    public interface IFourGuardiansBoardApi : ISlotBoardApi
    {
    }
}