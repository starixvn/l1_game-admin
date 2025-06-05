namespace GamesAdmin.Core.Models.Partners
{
    public class Partner
    {
        public Partner(string partnerId, string name, string secretKey, string clubVersionUrl, string mobiVersionUrl, string miniVersionUrl, string megaKenoUrl, string mobileDirectUrl, string testCurrencyUrl, string langliveUrl, bool isDisplayPlayerCurrency, string alias)
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
            LangliveUrl = langliveUrl;
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

        public string LangliveUrl { get; }

        public bool IsDisplayPlayerCurrency { get; }

        public string Alias { get; }
    }
}