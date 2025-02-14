namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features._Shared;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using GamesAdmin.Site.Features.RetrieveEndRound.Model;
    using Refit;

    public interface IRetrieveEndRoundApi
    {
        [Headers("Content-Type: application/json")]
        [Post("/Player/RetrieveEndRoundInfo")]
        Task<SGApiResponse<EndRoundResult>> RetrieveEndRound([Body(BodySerializationMethod.Serialized)] BetHistoryApiRequest request, [Header("Access-Key")] string accessKey);
    }
}