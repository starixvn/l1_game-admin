using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("admin_users")]
    public class AdminUserEntity : MiniGameBaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool Isadmin { get; set; }
    }
}
