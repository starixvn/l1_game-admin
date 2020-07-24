using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities.Spins
{
    [BsonCollection("member_spins")]
    public class SpinEntity : MiniGameBaseEntity
    {
        public SpinEntity()
        {
            Time = DateTime.UtcNow;
        }

        public string ObCustId { get; set; }

        public string SiteId { get; set; }

        public bool Used { get; set; }
    }
}
