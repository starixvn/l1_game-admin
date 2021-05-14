using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("sites")]
    public class ClientSiteEntity : Document
    {
        public string SiteId { get; set; }

        public string BackendApi { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string GameClientUrl { get; set; }

        public string GameMobiClientUrl { get; set; }
    }
}