namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features._Shared;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using Newtonsoft.Json;

    public interface IRetrieveEndRoundService
    {
        Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model);
    }

    public class RetrieveEndRoundService : IRetrieveEndRoundService
    {
        private readonly IRetrieveEndRoundApi retrieveEndRoundApi;

        public RetrieveEndRoundService(IRetrieveEndRoundApi retrieveEndRoundApi)
        {
            this.retrieveEndRoundApi = retrieveEndRoundApi;
        }

        public async Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model)
        {
            var request = new BetHistoryApiRequest(model);
            var accessKey = HmacHelpers.CreateAccessKey(request.Auth.OperatorSecret, request);
            var result = await retrieveEndRoundApi.RetrieveEndRound(request, accessKey);

            return JsonConvert.SerializeObject(result);
        }
            var hashResult = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
    }
}