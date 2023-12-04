using System.Threading.Tasks;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using Refit;

namespace GamesAdmin.Api.VoidGameCloud
{
    [Headers("Authorization: Bearer")]
    public interface IVoidGameCloudApi
    {
        [Post("/void/")]
        Task<VoidGameResult> VoidTicket([Body] VoidTicketRequest request);
    }

    public interface IVoidGameCloudService
    {
        Task<VoidGameResult> VoidCloudServiceTicket(VoidTicketRequest request);
    }

    public class VoidGameCloudService : IVoidGameCloudService
    {
        private readonly IVoidGameCloudApi voidGameApi;


        public VoidGameCloudService(IVoidGameCloudApi voidGameApi)
        {
            this.voidGameApi = voidGameApi;
        }


        public Task<VoidGameResult> VoidCloudServiceTicket(VoidTicketRequest request)
        => voidGameApi.VoidTicket(request);
    }
}
