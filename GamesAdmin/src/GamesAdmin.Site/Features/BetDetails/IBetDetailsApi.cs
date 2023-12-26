namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.Models;
    using Refit;

    public interface IBetDetailsApi
    {
        [Post("/BetHistory/GetAccessUrl")]
        Task<BetDetailsAccessUrlResult> GetAccessUrl([Body] BetDetailsAccessUrlApiRequest model);

        [Headers("Content-Type: application/json")]
        [Post("/BetHistory/GetAccessUrl")]
        Task<BetDetailsAccessUrlResult> GetAccessUrl([Body(BodySerializationMethod.Serialized)] BetDetailsAccessUrlApiRequest request, [Header("Access-Key")] string accessKey);
    }
}
