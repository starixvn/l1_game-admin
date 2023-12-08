using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using Refit;

namespace GamesAdmin.Api.VoidGameRemote
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameRemoteApi
    {
        [Post("/api/void")]
        Task<VoidGameResult> VoidTicket([Body] VoidTicketRequest request);
    }
}
