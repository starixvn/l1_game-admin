using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public abstract class MiniGameBaseEntity : Document
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Time { get; set; }

        public int __v { get; set; }
    }
}