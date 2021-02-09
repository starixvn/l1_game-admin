using System;
using GamesAdmin.Core.OWResults;
using GamesAdmin.Database.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("void_games")]
    public class VoidLogEntity : MiniGameBaseEntity
    {
        public VoidLogEntity(DateTimeOffset requestTimeStamp, VoidCall call, DateTimeOffset responseTimeStamp, VoidGameResult result)
        {
            LogTimeStamp = DateTimeOffset.UtcNow;
            RequestTimeStamp = requestTimeStamp;
            Call = call;
            ResponseTimeStamp = responseTimeStamp;
            Result = result;
        }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset LogTimeStamp { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset RequestTimeStamp { get; set; }

        public VoidCall Call { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset ResponseTimeStamp { get; set; }

        public VoidGameResult Result { get; set; }
    }

    public class VoidCall
    {
        public VoidCall(int obCustId, string siteId, long gameRoundId, byte gameTypeId, string reason, bool freeBet)
        {
            ObCustId = obCustId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
        }

        public int ObCustId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; }

        public byte GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}