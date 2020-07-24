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

        public string Message { get; set; }

        public byte DisplayKind { get; set; }
    }
}
