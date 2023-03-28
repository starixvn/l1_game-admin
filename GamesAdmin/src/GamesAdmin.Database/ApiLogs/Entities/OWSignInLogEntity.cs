using System;

namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class OWSignInLogEntity : ApiLogEntity
    {
        public OWSignInRequestEntity Request { get; set; }
        public OWSignInResultEntity Result { get; set; }
    }

    public class OWSignInRequestEntity
    {
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
        public OWSignInCallEntity Call { get; set; }
    }

    public class OWSignInCallEntity
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

    public class OWSignInResultEntity
    {
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string Seq { get; set; }
        public string RedirectUri { get; set; }
    }
}