namespace GamesAdmin.Core.Models.Partner
{
    public class PartnerInfo
    {
        public PartnerInfo(string partnerId, string name, string secretKey, string casinoVersionUrl, string mobiVersionUrl, string miniVersionUrl, string apiUrl)
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