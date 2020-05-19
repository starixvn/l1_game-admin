using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("event_schedule")]
    public class EventScheduleEntity : MiniGameBaseEntity
    {
        public EventScheduleEntity() 
        {
            Time = DateTimeOffset.UtcNow.UtcDateTime;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public byte EventType { get; set; }
    }
}
