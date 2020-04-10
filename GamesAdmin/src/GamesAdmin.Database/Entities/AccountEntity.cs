using System;
using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("users")]
    public class AccountEntity : MiniGameBaseEntity
    {
        public LocalAccount local { get; set; }
    }

    public class LocalAccount
    {
        public int ban_pass { get; set; }

        public bool ban_login { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public DateTime regDate { get; set; }

        public string memberId { get; set; }

        public string memberid { get; set; }

        public string siteId { get; set; }

        public string language { get; set; }
    }
}