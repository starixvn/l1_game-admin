using System.Collections.Generic;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("partner_configs")]
    public class PartnerConfigEntity : MiniGameBaseEntity
    {
        public string PartnerId { get; set; }

        public string Type { get; set; }

        public Dictionary<string, IList<string>> Value { get; set; }
    }
}