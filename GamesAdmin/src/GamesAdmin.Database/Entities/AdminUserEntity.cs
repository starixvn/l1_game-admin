using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("admin_users")]
    public class AdminUserEntity : MiniGameBaseEntity
    {
        public string username { get; set; }

        public string password { get; set; }

        public bool isadmin { get; set; }
    }
}
