using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("um_info")]
    public class UMInfoEntity : MiniGameBaseEntity
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Finish { get; set; }
    }
}
