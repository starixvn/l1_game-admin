namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features._Shared;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;

    public interface IBetDetailsService
    {
        Task<string> GetAccessUrlAsync(BetDetailsViewModel model);
    }

    public class BetDetailsService : IBetDetailsService
    {
        private readonly IBetDetailsApi betDetailsApi;
        private readonly IHmacHelpers hmacHelpers;

        public BetDetailsService(IBetDetailsApi betDetailsApi, IHmacHelpers hmacHelpers)
        {
            this.betDetailsApi = betDetailsApi;
            this.hmacHelpers = hmacHelpers;
        }
        public async Task<string> GetAccessUrlAsync(BetDetailsViewModel model)
        {
            var request = new BetDetailsAccessUrlApiRequest(model);

            if (model.UseHmacHash)
            {
                var accessKey = hmacHelpers.CreateAccessKey(model.OperatorSecret, request);
                var resultByHmac = await betDetailsApi.GetAccessUrl(request, accessKey);

                return resultByHmac.RedirectUrl;
            }

            var result = await betDetailsApi.GetAccessUrl(request);
            return result.RedirectUrl;

        }
    }
}
