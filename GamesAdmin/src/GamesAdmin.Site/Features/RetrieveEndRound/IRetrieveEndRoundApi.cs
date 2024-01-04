namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using Refit;

    public interface IRetrieveEndRoundApi
    {
        [Headers("Content-Type: application/json")]
        [Post("/Player/RetrieveEndRoundInfo")]
        Task<dynamic> RetrieveEndRound([Body(BodySerializationMethod.Serialized)] BetHistoryApiRequest request, [Header("Access-Key")] string accessKey);
    }
}