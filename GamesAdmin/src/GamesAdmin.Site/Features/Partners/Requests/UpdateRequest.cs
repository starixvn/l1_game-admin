using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateRequest : IRequest<ResultBase>
    {
        public UpdateRequest(string partnerId, string name, string secretKey, string clubVersionUrl, string mobiVersionUrl, string miniVersionUrl, string megaKenoUrl, string testCurrencyUrl)
        {
            PartnerId = partnerId;
            Name = name;
            SecretKey = secretKey;
            MobiVersionUrl = mobiVersionUrl;
            MiniVersionUrl = miniVersionUrl;
            ClubVersionUrl = clubVersionUrl;
            MegaKenoUrl = megaKenoUrl;
            TestCurrencyUrl = testCurrencyUrl;
        }

        public string PartnerId { get; }

        public string Name { get; }

        public string SecretKey { get; }

        public string ClubVersionUrl { get; }

        public string MobiVersionUrl { get; }

        public string MiniVersionUrl { get; }

        public string MegaKenoUrl { get; }

        public string TestCurrencyUrl { get; }
    }
}