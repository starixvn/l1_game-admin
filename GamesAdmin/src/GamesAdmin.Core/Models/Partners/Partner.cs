namespace GamesAdmin.Core.Models.Partners
{
    public class Partner
    {
        public Partner(string partnerId, string name, string secretKey, string clubVersionUrl, string mobiVersionUrl, string miniVersionUrl, string apiUrl, string testCurrencyUrl)
        {
            PartnerId = partnerId;
            Name = name;
            SecretKey = secretKey;
            MobiVersionUrl = mobiVersionUrl;
            MiniVersionUrl = miniVersionUrl;
            ClubVersionUrl = clubVersionUrl;
            ApiUrl = apiUrl;
            TestCurrencyUrl = testCurrencyUrl;

        }

        public string PartnerId { get; }

        public string Name { get; }

        public string SecretKey { get; }

        public string ClubVersionUrl { get; }

        public string MobiVersionUrl { get; }

        public string MiniVersionUrl { get; }

        public string ApiUrl { get; }

        public string TestCurrencyUrl { get; }
    }
}