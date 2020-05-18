using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("event_schedule")]
    public class EventSchedule : MiniGameBaseEntity
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
