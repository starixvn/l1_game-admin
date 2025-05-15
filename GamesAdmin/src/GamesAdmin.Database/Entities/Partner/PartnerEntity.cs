using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("partners")]
    public class PartnerEntity : Document
    {
        public string PartnerId { get; set; }

        public string Name { get; set; }

        public string SecretKey { get; set; }

        public string ClubVersionUrl { get; set; }

        public string MobiVersionUrl { get; set; }

        public string MiniVersionUrl { get; set; }

        public string MegaKenoUrl { get; set; }

        public string MobileDirectUrl { get; set; }

        public string TestCurrencyUrl { get; set; }

        public bool IsDisplayPlayerCurrency { get; set; }

        public string Alias { get; set; }
    }
}