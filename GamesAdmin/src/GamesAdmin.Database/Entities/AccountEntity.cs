using System;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("users")]
    public class AccountEntity : MiniGameBaseEntity
    {
        public LocalAccount Local { get; set; }
    }

    public class LocalAccount
    {
        public int Ban_pass { get; set; }

        public bool Ban_login { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime regDate { get; set; }

        public string MemberId { get; set; }

        public string MemberName { get; set; }

        public string SiteId { get; set; }

        public string Language { get; set; }
    }
}