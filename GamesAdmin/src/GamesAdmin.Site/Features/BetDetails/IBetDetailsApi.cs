namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using Refit;

    public interface IBetDetailsApi
    {
        [Post("/GameTicket/GetAccessUrl")]
        Task<BetDetailsAccessUrlResult> GetAccessUrl([Body] BetDetailsAccessUrlRequest model);
    }
}
