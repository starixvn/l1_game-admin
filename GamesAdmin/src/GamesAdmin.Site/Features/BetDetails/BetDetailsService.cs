namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Threading.Tasks;
    using GamesAdmin.Core.Models.ApiLogs;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

    public interface IBetDetailsService
    {
        Task<string> GetAccessUrlAsync(BetDetailsViewModel model);
    }

    public class BetDetailsService : IBetDetailsService
    {
        private readonly IBetDetailsApi betDetailsApi;

        public BetDetailsService(IBetDetailsApi betDetailsApi)
        {
            this.betDetailsApi = betDetailsApi;
        }
        public async Task<string> GetAccessUrlAsync(BetDetailsViewModel model)
        {
            var result = await betDetailsApi.GetAccessUrl(new BetDetailsAccessUrlRequest(model));

            return result.RedirectUrl;
        }
    }
}
