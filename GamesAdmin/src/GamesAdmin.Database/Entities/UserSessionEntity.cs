using GamesAdmin.Database.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("usersessions")]
    public class UserSessionEntity : Document
    {
        [BsonElement("recheckInTime")]
        public DateTime RecheckInTime { get; set; }
    }
}