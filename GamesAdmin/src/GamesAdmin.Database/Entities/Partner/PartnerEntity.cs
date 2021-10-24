using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("partners")]
    public class PartnerEntity : Document
    {
        [BsonElement("id")]
        public string PartnerId { get; set; }

        public string Name { get; set; }

        public string SecretKey { get; set; }

        public string CasinoVersionUrl { get; set; }

        public string MobiVersionUrl { get; set; }

        public string MiniVersionUrl { get; set; }

        public string ApiUrl { get; set; }

        public string TestCurrencyUrl { get; set; }
    }
}