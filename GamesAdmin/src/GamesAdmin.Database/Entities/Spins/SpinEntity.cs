using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities.Spins
{
    [BsonCollection("user_spins")]
    public class SpinEntity : MiniGameBaseEntity
    {
        public SpinEntity()
        {
            Time = DateTime.UtcNow;
        }

        public string ObCustId { get; set; }

        public bool Used { get; set; }
    }
}
