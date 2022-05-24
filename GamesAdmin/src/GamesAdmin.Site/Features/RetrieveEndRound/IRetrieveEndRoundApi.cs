namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.RetrieveEndRound.Model;
    using Refit;
    public interface IRetrieveEndRoundApi
    {
        [Headers("Content-Type: application/json")]
        [Post("/Player/RetrieveEndRoundInfo")]
        Task<dynamic> RetrieveEndRound([Body(BodySerializationMethod.Serialized)] RetrieveEndRoundRequest request, [Header("Access-Key")] string accessKey);
    }
}
