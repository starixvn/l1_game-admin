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

        public string SiteId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordEncodeKey { get; set; }

        public string Seq { get; set; }

        public string Platform { get; set; }
    }

    public class MemberSignInResult
    {
        public string ErrorMsg { get; set; }
        public int ErrorCode { get; set; }
        public PublicUserInfo PublicUserInfo { get; set; }
    }

    public class PublicUserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string Currency { get; set; }
    }
}