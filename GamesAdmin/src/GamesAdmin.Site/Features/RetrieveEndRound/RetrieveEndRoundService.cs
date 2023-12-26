
namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features._Shared;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using Newtonsoft.Json;

    public interface IRetrieveEndRoundService
    {
        Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model);
    }

    public class RetrieveEndRoundService : IRetrieveEndRoundService
    {
        private readonly IRetrieveEndRoundApi retrieveEndRoundApi;
        private readonly IHmacHelpers hmacHelpers;

        public RetrieveEndRoundService(IRetrieveEndRoundApi retrieveEndRoundApi, IHmacHelpers hmacHelpers)
        {
            this.retrieveEndRoundApi = retrieveEndRoundApi;
            this.hmacHelpers = hmacHelpers;
        }

        public async Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model)
        {
            var request = new BetHistoryApiRequest(model);
            var accessKey = hmacHelpers.CreateAccessKey(request.Auth.OperatorSecret, request);
            var result = await retrieveEndRoundApi.RetrieveEndRound(request, accessKey);

            return JsonConvert.SerializeObject(result);
        }
    }
}
