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

        [BsonElement("__archive")]
        public ApiLogArchiveEntity Archive { get; set; }
    }

    public abstract class ApiLogRequestEntity
    {
        [BsonElement("_t")]
        public virtual string RequestType { get; set; }

        public int GameRoundId { get; set; }

        public int? GameTypeId { get; set; }

        public int PlayerId { get; set; }
    }

    public abstract class ApiLogResultEntity
    {
        [BsonElement("_t")]
        public virtual string ResultType { get; set; }
    }

    public class ApiLogArchiveEntity
    {
        [BsonElement("betlogs")]
        public int BetLogs { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement("eventtime")]
        public DateTime EventTime { get; set; }
    }
}