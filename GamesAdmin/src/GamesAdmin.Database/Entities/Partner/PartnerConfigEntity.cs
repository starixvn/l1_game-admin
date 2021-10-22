using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("partnerConfig")]
    public class PartnerConfigEntity : MiniGameBaseEntity
    {
        public string PartnerId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int GameId { get; set; }
    }
}