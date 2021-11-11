using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public abstract class ApiLogEntity : Document
    {
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset LogTimeStamp { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset RequestTimeStamp { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset ResponseTimeStamp { get; set; }
    }

    public abstract class ApiLogRequestEntity
    {
        [BsonElement("_t")]
        public virtual string RequestType { get; set; }

        public int GameRoundId { get; set; }

        public byte? GameTypeId { get; set; }

        public int ObCustId { get; set; }
    }

    public abstract class ApiLogResultEntity
    {
        [BsonElement("_t")]
        public virtual string ResultType { get; set; }
    }
}