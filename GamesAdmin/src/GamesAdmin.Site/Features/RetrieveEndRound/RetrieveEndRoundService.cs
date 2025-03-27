namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Extensions;
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
        private readonly IPartnerPlayerIdConverter playerIdConverter;

        public RetrieveEndRoundService(IRetrieveEndRoundApi retrieveEndRoundApi, IPartnerPlayerIdConverter playerIdConverter)
        {
            this.retrieveEndRoundApi = retrieveEndRoundApi;
            this.playerIdConverter = playerIdConverter;
        }

        public async Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model)
        {
            var convertedPlayerId = playerIdConverter.ConvertPlayerId(model.OperatorId, model.PlayerId);

            var request = new BetHistoryApiRequest(model)
            {
                PlayerId = convertedPlayerId
            };
            var accessKey = HmacHelpers.CreateAccessKey(request.Auth.OperatorSecret, request);
            var result = await retrieveEndRoundApi.RetrieveEndRound(request, accessKey);

            return JsonConvert.SerializeObject(result);
        }
    }
}