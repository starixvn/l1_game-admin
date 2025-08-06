using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateRequest : IRequest<ResultBase>
    {
        public UpdateRequest(string partnerId, string name, string secretKey, string clubVersionUrl, string mobiVersionUrl, string miniVersionUrl, string megaKenoUrl, string mobileDirectUrl, string testCurrencyUrl, string livestreamingPlatformUrl, string originalGamesUrl, bool isDisplayPlayerCurrency, string alias)
        {
            PartnerId = partnerId;
            Name = name;
            SecretKey = secretKey;
            MobiVersionUrl = mobiVersionUrl;
            MiniVersionUrl = miniVersionUrl;
            ClubVersionUrl = clubVersionUrl;
            MegaKenoUrl = megaKenoUrl;
            MobileDirectUrl = mobileDirectUrl;
            TestCurrencyUrl = testCurrencyUrl;
            LivestreamingPlatformUrl = livestreamingPlatformUrl;
            OriginalGamesUrl = originalGamesUrl;
            IsDisplayPlayerCurrency = isDisplayPlayerCurrency;
            Alias = alias;
        }

        public string PartnerId { get; }

        public string Name { get; }

        public string SecretKey { get; }

        public string ClubVersionUrl { get; }

        public string MobiVersionUrl { get; }

        public string MiniVersionUrl { get; }

        public string MegaKenoUrl { get; }

        public string MobileDirectUrl { get; }

        public string TestCurrencyUrl { get; }

        public string LivestreamingPlatformUrl { get; }

        public string OriginalGamesUrl { get; }

        public bool IsDisplayPlayerCurrency { get; }

        public string Alias { get; }
    }
}