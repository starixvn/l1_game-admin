namespace GamesAdmin.Site.Features.DailySummary
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.DailySummary.Models;
    using Refit;

    public interface IDailySummaryApi
    {
        [Post("/Member/DailySummarize")]
        Task<dynamic> GetDailySummary([Body(BodySerializationMethod.Serialized)] DailySummaryRequest request);
    }
}
