
namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using GamesAdmin.Site.Features.RetrieveEndRound.Model;
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
            var request = new RetrieveEndRoundRequest(model);
            var result = await retrieveEndRoundApi.RetrieveEndRound(request);

            return JsonConvert.SerializeObject(result);
        }
    }
}
