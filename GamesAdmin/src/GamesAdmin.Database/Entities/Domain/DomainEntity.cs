using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities
{
    [BsonCollection("domains")]
    public class DomainEntity : MiniGameBaseEntity
    {
        public string Value { get; set; } = string.Empty;
    }
}
