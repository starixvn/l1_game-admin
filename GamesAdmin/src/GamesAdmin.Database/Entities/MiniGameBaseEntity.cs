using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    public abstract class MiniGameBaseEntity : Document
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime time { get; set; }

        public int __v { get; set; }
    }
}