using GamesAdmin.Database.Attributes;

namespace GamesAdmin.Database.Entities.Currencies
{
    [BsonCollection("currencies")]
    public class CurrencyEntity : MiniGameBaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public double Rate { get; set; }

        public bool Enabled { get; set; }

        public bool IsTest { get; set; }

        public string PartnerId { get; set; }

        public string CurrencyGroupId { get; set; }
    }
}