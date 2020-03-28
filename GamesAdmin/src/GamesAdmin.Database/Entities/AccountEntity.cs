using GamesAdmin.Database.Attributes;
using System;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("users")]
    public class AccountEntity : Document
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
    }
}
