using System;

namespace GamesAdmin.Core.Models.ApiLogs
{
    public class MemberSignInLog : IApiLog
    {
        public DateTimeOffset LogTimeStamp { get; set; }
        public DateTimeOffset RequestTimeStamp { get; set; }
        public DateTimeOffset ResponseTimeStamp { get; set; }
        public IApiLogArchive Archive { get; set; }

        public MemberSignInRequest Request { get; set; }
        public MemberSignInResult Result { get; set; }
    }

    public class MemberSignInRequest
    {
        public string VendorId { get; set; }

        public string Username { get; set; }

        public string Platform { get; set; }

        public string SiteId { get; set; }

        public string Seq { get; set; }
    }

    public class MemberFailedSignInRequest : MemberSignInRequest, IApiLogRequest
    {

        public object PlayerId { get; set; }

        public long? GameRoundId { get; set; }

        public int? GameTypeId { get; set; }

        public string RequestType { get; set; }
    }

    public class MemberSignInResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string Seq { get; set; }
        public PublicUserInfo PublicUserInfo { get; set; }
    }

    public class MemberFailedSignInResult : MemberSignInResult, IPartnerResult
    {
        public string ResultType { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDescription { get; set; }

        public object PlayerId { get; set; }
    }

    public class PublicUserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string AgentSite { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string Currency { get; set; }
    }
}