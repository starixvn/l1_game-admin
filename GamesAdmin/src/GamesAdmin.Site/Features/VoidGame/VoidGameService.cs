using System.Threading.Tasks;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.VoidGame.Requests;
using Refit;

namespace GamesAdmin.Site.Features.VoidGame
{
    public interface IVoidGameApi : IBaseAuthorizationApi
    {
        [Post("/void/")]
        Task<VoidGameResult> VoidTicket([Body] VoidTicketRequest request);
    }

    public interface IVoidGameService
    {
        Task<VoidGameResult> VoidTicket(VoidTicketRequest request);
    }

    public class VoidGameService : IVoidGameService
    {
        private readonly IVoidGameApi voidGameApi;

        public VoidGameService(IVoidGameApi voidGameApi)
        {
            this.voidGameApi = voidGameApi;
        }

        public Task<VoidGameResult> VoidTicket(VoidTicketRequest request)
        => voidGameApi.VoidTicket(request);
    }
}