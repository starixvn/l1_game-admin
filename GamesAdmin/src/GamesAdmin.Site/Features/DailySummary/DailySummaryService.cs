namespace GamesAdmin.Site.Features.DailySummary
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.DailySummary.Models;
    using GamesAdmin.Site.Features.DailySummary.ViewModels;
    using Newtonsoft.Json;

    public interface IDailySummaryService
    {
        Task<string> GetDailySummarydAsync(DailySummaryViewModel model);
    }

    public class DailySummaryService : IDailySummaryService
    {
        private readonly IDailySummaryApi dailySummaryApi;

        public DailySummaryService(IDailySummaryApi dailySummaryApi)
        {
            this.dailySummaryApi = dailySummaryApi;
        }

        public async Task<string> GetDailySummarydAsync(DailySummaryViewModel model)
        {
            var request = new DailySummaryRequest(model);
            var result = await dailySummaryApi.GetDailySummary(request);

            return JsonConvert.SerializeObject(result);
        }
    }
}
