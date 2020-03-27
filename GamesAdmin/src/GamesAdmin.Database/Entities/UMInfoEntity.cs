using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("um_info")]
    public class UMInfoEntity : Document
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public bool finish { get; set; }
    }
}
