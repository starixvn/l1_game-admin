
namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using GamesAdmin.Site.Features.RetrieveEndRound.Model;
    using Newtonsoft.Json;

    public interface IRetrieveEndRoundService
    {
        Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model);
    }

    public class RetrieveEndRoundService : IRetrieveEndRoundService
    {
        private readonly IRetrieveEndRoundApi retrieveEndRoundApi;

        public RetrieveEndRoundService(IRetrieveEndRoundApi retrieveEndRoundApi)
        {
            this.retrieveEndRoundApi = retrieveEndRoundApi;
        }

        public async Task<string> GetRetrieveEndRoundAsync(BetDetailsViewModel model)
        {
            var request = new RetrieveEndRoundRequest(model);
            var requestBody = JsonConvert.SerializeObject(request);
            var accessKey = CreateAccessKey(request.Auth.OperatorSecret, requestBody);
            var result = await retrieveEndRoundApi.RetrieveEndRound(request, accessKey);

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
