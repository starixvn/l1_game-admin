using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Api.VoidGame.Results;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Api.VoidGame
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameApi
    {
        [Get("/SabaClub/VoidGame")]
        Task<VoidGameResult> VoidGameTicket([FromBody] VoidTicketRequest request);
    }

    public interface IVoidGameService
    {
        Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request);
    }

    public class VoidGameService : IVoidGameService
    {
        private readonly IVoidGameApi voidGameApi;

        public VoidGameService(IVoidGameApi voidGameApi)
        {
            this.voidGameApi = voidGameApi;
        }

        public Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request)
        => voidGameApi.VoidGameTicket(request);
    }
}