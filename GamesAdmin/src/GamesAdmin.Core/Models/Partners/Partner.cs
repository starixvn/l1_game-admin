namespace GamesAdmin.Core.Models.Partners
{
    public class Partner
    {
        public Partner(string partnerId, string name, string secretKey, string clubVersionUrl, string mobiVersionUrl, string miniVersionUrl, string megaKenoUrl, string desktopLiveUrl, string mobileDirectUrl, string desktopTodayUrl, string testCurrencyUrl, string livestreamingPlatformUrl, bool isDisplayPlayerCurrency, string alias)
        {
            PartnerId = partnerId;
            Name = name;
            SecretKey = secretKey;
            MobiVersionUrl = mobiVersionUrl;
            MiniVersionUrl = miniVersionUrl;
            ClubVersionUrl = clubVersionUrl;
            MegaKenoUrl = megaKenoUrl;
            DesktopLiveUrl = desktopLiveUrl;
            MobileDirectUrl = mobileDirectUrl;
            DesktopTodayUrl = desktopTodayUrl;
            TestCurrencyUrl = testCurrencyUrl;
            LivestreamingPlatformUrl = livestreamingPlatformUrl;
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

        public string DesktopLiveUrl { get; }

        public string MobileDirectUrl { get; }

        public string DesktopTodayUrl { get; }

        public string TestCurrencyUrl { get; }

        public string LivestreamingPlatformUrl { get; }

        public bool IsDisplayPlayerCurrency { get; }

        public string Alias { get; }
    }
}