using System;
using GamesAdmin.Core.Models.ApiLogs;

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

        public string Domain { get; set; }

        public string BrowserUserAgent { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int MemberId { get; set; }

        public byte AppType { get; set; }

        public Auth Auth { get; set; }
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
