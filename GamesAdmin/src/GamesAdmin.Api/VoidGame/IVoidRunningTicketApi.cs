using GamesAdmin.Api.VoidGame.Requests;
using Refit;
using System.Threading.Tasks;

namespace GamesAdmin.Api.VoidGame
{
    [Headers("Authorization: Bearer")]
    public interface IVoidRunningTicketApi
    {
        [Post("/api/void-ticket")]
        Task<bool> VoidTicket([Body] VoidRunningTicketRequest request);
    }
}
