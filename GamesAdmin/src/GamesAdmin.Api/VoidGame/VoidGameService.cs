using System;
using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Api.VoidGame
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameApi
    {
        [Post("/Member/VoidGame")]
        Task<VoidGameResult> VoidGameTicket([Body] VoidTicketRequest request);
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

        public async Task<VoidGameResult> VoidGameTicket(VoidTicketRequest request)
        {
            try
            {
                var result = await voidGameApi.VoidGameTicket(request);

                return result;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return default;
        }
    }
}