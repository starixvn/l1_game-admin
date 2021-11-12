using System;
using GamesAdmin.Core.Models.ApiLogs;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class SBLogEntity : ApiLogEntity
    {
        public SBRequestEntity Request { get; set; }

        public SBResultEntity Result { get; set; }
    }

    public class SBRequestEntity : ApiLogRequestEntity
    {
        public string Seq { get; set; }

        public string SiteId { get; set; }

        public string Domain { get; set; }

        public string BrowserUserAgent { get; set; }

        public string Currency { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public byte AppType { get; set; }

        public string SiteName { get; set; }

        public Auth Auth { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public bool FreeBet { get; set; }
    }

    public class SBResultEntity : ApiLogResultEntity
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string RedirectUrl { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalWin { get; set; }

        public decimal ValidBetAmount { get; set; }

        public string Seq { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}