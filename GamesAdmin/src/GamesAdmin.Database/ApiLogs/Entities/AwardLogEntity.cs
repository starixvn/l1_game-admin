using GamesAdmin.Database.ApiLogs.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    internal class AwardLogEntity : ApiLogEntity
    {
        public AwardLogRequestEntity Request { get; set; }

        public AwardLogResultEntity Result { get; set; }
    }

    public class AwardLogRequestEntity : ApiLogRequestEntity
    {
        public string Seq { get; set; }
        public string SiteId { get; set; }
        public string Amount { get; set; }

        [BsonSerializer(typeof(PlayerIdSerializer))]
        public object PromotionId { get; set; }
        public string Currency { get; set; } 
    }

    public class AwardLogResultEntity : ApiLogResultEntity
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Seq { get; set; }       

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}
