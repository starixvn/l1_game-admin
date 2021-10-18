using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateRequest : IRequest<ResultBase>
    {
        public UpdateRequest(string partnerId, string name, string secretKey, string casinoVersionUrl, string mobiVersionUrl, string miniVersionUrl, string apiUrl)
        {
            PartnerId = partnerId;
            Name = name;
            SecretKey = secretKey;
            MobiVersionUrl = mobiVersionUrl;
            MiniVersionUrl = miniVersionUrl;
            CasinoVersionUrl = casinoVersionUrl;
            ApiUrl = apiUrl;
        }

        public string PartnerId { get; }

        public string Name { get; }

        public string SecretKey { get; }

        public string CasinoVersionUrl { get; }

        public string MobiVersionUrl { get; }

        public string MiniVersionUrl { get; }

        public string ApiUrl { get; }
    }
}
