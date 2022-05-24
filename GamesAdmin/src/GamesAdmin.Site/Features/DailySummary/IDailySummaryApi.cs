namespace GamesAdmin.Site.Features.DailySummary
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.DailySummary.Models;
    using Refit;

    public interface IDailySummaryApi
    {
        [Headers("Content-Type: application/json")]
        [Post("/Integrity/DailySummarize")]
        Task<dynamic> GetDailySummary([Body(BodySerializationMethod.Serialized)] DailySummaryRequest request, [Header("Access-Key")] string accessKey);
    }
}
