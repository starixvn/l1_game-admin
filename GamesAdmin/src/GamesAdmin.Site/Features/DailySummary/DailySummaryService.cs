namespace GamesAdmin.Site.Features.DailySummary
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
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
            var requestBody = JsonConvert.SerializeObject(request);
            var accessKey = CreateAccessKey(request.Auth.OperatorSecret, requestBody);

            var result = await dailySummaryApi.GetDailySummary(request, accessKey);

            return JsonConvert.SerializeObject(result);
        }

        private string CreateAccessKey(string secretKey, string data)
        {

            var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(secretKey));
            var hashResult = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));

            return Convert.ToBase64String(hashResult);
        }
    }
}
