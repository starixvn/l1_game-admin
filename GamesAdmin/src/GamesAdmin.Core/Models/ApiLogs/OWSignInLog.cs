using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class OWSignInLog : IApiLog
    {
        public DateTimeOffset LogTimeStamp { get; set; }
        public DateTimeOffset RequestTimeStamp { get; set; }
        public DateTimeOffset ResponseTimeStamp { get; set; }
        public IApiLogArchive Archive { get; set; }

        public OWSignInRequest Request { get; set; }
        public OWSignInResult Result { get; set; }
    }

    public class OWSignInRequest
    {
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
        public OWSignInCall Call { get; set; }
    }

    public class OWSignInCall
    {
        public int ObCustId { get; set; }
        public string ObCustName { get; set; }
        public int GameId { get; set; }
        public string Currency { get; set; }
        public string IP { get; set; }
        public string BrowserUserAgent { get; set; }
        public string BetFrom { get; set; }
        public string Language { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string Seq { get; set; }
    }

    public class OWFailedSignInCall : OWSignInCall, IApiLogRequest
    {
        public new int? ObCustId { get; set; }
        public int? PlayerId { get; set; }
        public int? GameRoundId { get; set; }
        public byte? GameTypeId { get; set; }
        public string RequestType { get; set; }
    }

    public class OWSignInResult
    {
        public string RedirectUri { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string Seq { get; set; }
    }

    public class OWFailedSignInResult : OWSignInResult, IApiLogResult
    {
        public string ResultType { get; set; }
    }
}