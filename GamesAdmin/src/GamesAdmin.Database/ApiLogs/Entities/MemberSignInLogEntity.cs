namespace GamesAdmin.Database.ApiLogs.Entities
{
    public class MemberSignInLogEntity : ApiLogEntity
    {
        public MemberSignInRequestEntity Request { get; set; }    
        public MemberSignInResultEntity Result { get; set; }
    }

    public class MemberSignInRequestEntity
    {
        public string VendorId { get; set; }

        public string SiteId { get; set; }

        public string Username { get; set; }

        public string Seq { get; set; }

        public string Platform { get; set; }
    }

    public class MemberSignInResultEntity
    {
        public string ErrorMsg { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string Seq { get; set; }
        public PublicUserInfoEntity PublicUserInfo { get; set; }
    }

    public class PublicUserInfoEntity
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public UserInfoEntity UserInfo { get; set; }
    }

    public class UserInfoEntity
    {
        public string Currency { get; set; }
    }
}