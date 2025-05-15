namespace GamesAdmin.Site.Features.DailySummary
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features._Shared;
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
            var accessKey = HmacHelpers.CreateAccessKey(request.Auth.OperatorSecret, request);
            var result = await dailySummaryApi.GetDailySummary(request, accessKey);

            return JsonConvert.SerializeObject(result);
        }
    }
}


